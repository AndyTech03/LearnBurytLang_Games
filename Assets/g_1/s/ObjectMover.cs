using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    [SerializeField] private GameObject[] Path_Points;
    [SerializeField] private AnimationCurve MoveCurve;
    [SerializeField] private AnimationCurve RotateCurve;

    [NonReorderable] private GameObject TargetObject;
    private MoveStatus Status;
    private LinkedList<int> PathNodes;
    private Node<int> CurentNode;

    private int _lenght;
    private float _progress;

    private bool Notify_EndReaching;
    private bool Notify_StartReaching;

    public Action EndReaching_Notification;
    public Action StartReaching_Notification;

    public class Node<T>
    {
        public Node(T data)
        {
            Next = Previous = null;
            Data = data;
        }
        public T Data { get; set; }
        public Node<T> Next { get; set; }
        public Node<T> Previous { get; set; }
    }
    public class LinkedList<T> : IEnumerable<T>
    {
        Node<T> head; 
        Node<T> tail; 
        int count;

        public Node<T> First => head;
        public Node<T> Last => tail;

        public int Count => count;
        public bool IsEmpty => count == 0;

        public void Add(T data)
        {
            Node<T> node = new Node<T>(data);

            if (head == null)
                head = node;
            else
            {
                tail.Next = node;
                node.Previous = tail;
            }
            tail = node;

            count++;
        }
        public void Clear()
        {
            head = null;
            tail = null;
            count = 0;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            Node<T> current = head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
    }

    /// <summary>
    /// Return true if <see cref="TargetObject"/> is moving
    /// </summary>
    public bool ObjectIsMoving => TargetIsMoving_Test();

    enum MoveStatus
    {
        NoTarget,
        Idle,
        MovingForvard,
        MovingBackvard,
        ForvardStoped,
        BackvardStoped,
    }

    private void Awake()
    {
        _lenght = Path_Points.Length;

        if (_lenght != RotateCurve.keys.Length || _lenght != MoveCurve.keys.Length)
            throw new Exception($"Wrong configs! (lenght) {gameObject.name} {_lenght} {RotateCurve.keys.Length} {MoveCurve.keys.Length}");

        if (RotateCurve.keys[_lenght - 1].value != MoveCurve.keys[_lenght - 1].value)
            throw new Exception("Wrong configs! (value)" + gameObject.name);

        if (RotateCurve.keys[_lenght - 1].time != MoveCurve.keys[_lenght - 1].time)
            throw new Exception("Wrong configs! (time)" + gameObject.name);

        PathNodes = new LinkedList<int>();
        for (int i = 0; i < _lenght; i++)
            PathNodes.Add(i);
        Status = MoveStatus.NoTarget;
    }

    private void MoveToNode(int index1, int index2)
    {
        Vector3 pos1 = Path_Points[index1].transform.position;
        Vector3 pos2 = Path_Points[index2].transform.position;
        Vector3 absolute_deltaMove = pos2 - pos1;
        float move_value_delta = MoveCurve.keys[index2].value - MoveCurve.keys[index1].value;
        float evoluation = (MoveCurve.Evaluate(_progress) - MoveCurve.keys[index1].value) / move_value_delta;
        Vector3 point = pos1 + absolute_deltaMove * evoluation;
        TargetObject.transform.position = point;

        Vector3 rotation1 = Path_Points[index1].transform.rotation.eulerAngles;
        Vector3 rotation2 = Path_Points[index2].transform.rotation.eulerAngles;
        Vector3 absolute_deltaRotation = rotation2 - rotation1;
        float rotate_value_delta = RotateCurve.keys[index2].value - RotateCurve.keys[index1].value;
        evoluation = (RotateCurve.Evaluate(_progress) - RotateCurve.keys[index1].value) / rotate_value_delta;
        Vector3 euler = rotation1 + absolute_deltaRotation * evoluation;
        TargetObject.transform.rotation = Quaternion.Euler(euler);

    }

    private void FixedUpdate()
    {
        if (ObjectIsMoving)
        {
            switch (Status)
            {
                case MoveStatus.MovingForvard:
                    {
                        _progress += Time.fixedDeltaTime;

                        int index1 = CurentNode.Data;
                        int index2 = CurentNode.Next.Data;

                        MoveToNode(index1, index2);
                        if (_progress >= MoveCurve.keys[index2].time * 0.99)
                        {   
                            CurentNode = CurentNode.Next;
                            SetTargetToNode(CurentNode.Data);
                            if (CurentNode.Next == null)
                            {
                                FinishMoving(true);
                            }
                        }
                        break;
                    }

                case MoveStatus.MovingBackvard:
                    {
                        _progress -= Time.fixedDeltaTime;

                        int index1 = CurentNode.Data;
                        int index2 = CurentNode.Previous.Data;

                        MoveToNode(index1, index2);
                        if (_progress <= MoveCurve.keys[index2].time * 0.99)
                        {
                            CurentNode = CurentNode.Previous;
                            SetTargetToNode(CurentNode.Data);
                            if (CurentNode.Previous == null)
                            {
                                FinishMoving(false);
                            }
                        }
                        break;
                    }
            }
        }
    }

    private bool CurentNodeIsSet_Test()
    {
        if (CurentNode is null)
        {
            return false;
        }

        return true;
    }

    private bool TargetIsSet_Test()
    {
        if (TargetObject is null)
        {
            if (Status == MoveStatus.NoTarget)
            {
                return false;
            }
            else
            {
                throw new Exception("No target and wrong status!");
            }
        }
        return true;
    }

    private bool TargetIsMoving_Test()
    {
        if (Status == MoveStatus.MovingBackvard ||
            Status == MoveStatus.MovingForvard)
        {
            return true;
        }
        return false;
    }

    private void SetTargetToNode(int index)
    {
        TargetObject.transform.SetParent(Path_Points[index].transform);
        TargetObject.transform.localPosition = Vector3.zero;
        _progress = MoveCurve.keys[index].time;
    }

    private void FinishMoving(bool is_end)
    {
        Status = MoveStatus.Idle;
        if (Notify_EndReaching && is_end)
        {
            EndReaching_Notification?.Invoke();
        }

        if (Notify_StartReaching && is_end == false)
        {
            StartReaching_Notification?.Invoke();
        }
    }

    /// <summary>
    /// Set new <see cref="TargetObject"/> on <see cref="PathNodes"/> first point
    /// </summary>
    /// <param name="target">Seting <see cref="TargetObject"/></param>
    public void SetOnStart(GameObject target = null)
    {
        if (target == null)
        {
            if (TargetIsSet_Test() == false)
                throw new Exception("Nothing to set!");
        }
        else
        {
            if (TargetIsSet_Test())
                throw new Exception("Target is seted yet!");

            TargetObject = target;
        }

        Status = MoveStatus.Idle;
        CurentNode = PathNodes.First;
        SetTargetToNode(CurentNode.Data);
    }
    /// <summary>
    /// Set new <see cref="TargetObject"/> on <see cref="PathNodes"/> last point
    /// </summary>
    /// <param name="target">Seting <see cref="TargetObject"/></param>
    public void SetInEnd(GameObject target = null)
    {
        if (target == null)
        {
            if (TargetIsSet_Test() == false)
                throw new Exception("Nothing to set!");
        }
        else
        {
            if (TargetIsSet_Test())
                throw new Exception("Target is seted yet!");

            TargetObject = target;
        }

        Status = MoveStatus.Idle;
        CurentNode = PathNodes.Last;
        SetTargetToNode(CurentNode.Data);
    }

    /// <summary>
    /// Get <see cref="TargetObject"/> and clear the <see cref="ObjectMover"/>
    /// </summary>
    /// <returns><see cref="TargetObject"/></returns>
    public GameObject Get_Object()
    {
        if (TargetIsSet_Test() == false)
        {
            throw new Exception("Nothing to get!");
        }

        if (TargetIsMoving_Test())
        {
            throw new Exception("Need to stop at first!");
        }

        Status = MoveStatus.NoTarget;
        GameObject target = TargetObject;
        TargetObject = null;
        CurentNode = null;
        return target;
    }

    /// <summary>
    /// Begin forvard moving process
    /// </summary>
    /// <param name="notyfy">if true notify by Action <see cref="EndReaching_Notification"/></param>
    public void MoveForvard(bool notyfy = false)
    {
        if (TargetIsSet_Test() == false)
            throw new Exception("Nothing to move!");

        if (CurentNodeIsSet_Test() == false)
            throw new Exception("Path not seted!");

        switch (Status)
        {
            case MoveStatus.BackvardStoped:
                {
                    if (CurentNode.Previous != null)
                    {
                        CurentNode = CurentNode.Previous;
                    }
                    break;
                }
        }

        if (CurentNode.Next != null)
        {
            Status = MoveStatus.MovingForvard;
            Notify_EndReaching = notyfy;
        }
    }

    /// <summary>
    /// Begin backvard moving process
    /// </summary>
    /// <param name="notyfy">if true notify by Action <see cref="StartReaching_Notification"/></param>
    public void MoveBackvard(bool notyfy = false)
    {
        if (TargetIsSet_Test() == false)
            throw new Exception("Nothing to move!");

        if (CurentNodeIsSet_Test() == false)
            throw new Exception("Path not seted!");
        switch (Status)
        {
            case MoveStatus.ForvardStoped:
                {
                    if (CurentNode.Next != null)
                    {
                        CurentNode = CurentNode.Next;
                    }

                    break;
                }
        }

        if (CurentNode.Previous != null)
        {
            Status = MoveStatus.MovingBackvard;
            Notify_StartReaching = notyfy;
        }
    }

    /// <summary>
    /// Stop moving process
    /// </summary>
    public void StopMoving()
    {
        if (TargetIsSet_Test() == false)
            throw new Exception("Nothing to stop!");

        switch (Status)
        {
            case MoveStatus.MovingForvard:
                {
                    Status = MoveStatus.ForvardStoped;
                    Notify_EndReaching = false;
                    break;
                }
            case MoveStatus.MovingBackvard:
                {
                    Status = MoveStatus.BackvardStoped;
                    Notify_StartReaching = false;
                    break;
                }
        }

    }

    /// <summary>
    /// Run <see cref="SetOnStart(GameObject)"/> and <see cref="MoveForvard(bool)"/>
    /// </summary>
    /// <param name="target">Argument sending into <see cref="SetOnStart(GameObject)"/></param>
    /// <param name="notyfy">Argument sending into <see cref="MoveForvard(bool)"/></param>
    public void MoveForvard_FromStart(GameObject target = null, bool notyfy = false)
    {
        if(target == null)
        {
            if (TargetIsSet_Test() == false)
                throw new Exception("Nothing to move!");

            target = TargetObject;
        }

        if (TargetIsMoving_Test())
            throw new Exception("Is Busy!");

        SetOnStart(target);
        MoveForvard(notyfy);
    }

    /// <summary>
    /// Run <see cref="SetInEnd(GameObject)"/> and <see cref="MoveBackvard(bool)"/>
    /// </summary>
    /// <param name="target">Argument sending into <see cref="SetInEnd(GameObject)"/></param>
    /// <param name="notyfy">Argument sending into <see cref="MoveBackvard(bool)"/></param>
    public void MoveBackvard_FromFinish(GameObject target = null, bool notyfy = false)
    {
        if (target == null)
        {
            if (TargetIsSet_Test() == false)
                throw new Exception("Nothing to move!");

            target = TargetObject;
        }

        if (TargetIsMoving_Test())
            throw new Exception("Is Busy!");

        SetInEnd(target);
        MoveBackvard(notyfy);
    }
}

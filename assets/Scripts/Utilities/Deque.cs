using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deque<T> {

	List<T> InternalList;
	public int Count = 0;

	public Deque(){
		InternalList = new List<T>();
		Stuff f = new Stuff();
	}

	public void Enqueue(T Thing){
		InternalList.Add(Thing);
		Count++;
	}
	public void EnqueueFirst(T Thing){
		InternalList.Insert(0, Thing);
		Count++;
	}
	public void EnqueueRange(List<T> Things){
		foreach (T Thing in Things)
		{
			InternalList.Add(Thing);
			Count++;
		}
	}
	public void EnqueueRange(Deque<T> Things){
		for (int i = 0; i < Things.Count; i++)
		{
			InternalList.Add(Things.At(i));
		}
	}
	public T Dequeue(){
		T Thing = InternalList[0];
		InternalList.RemoveAt(0);
		Count--;
		return Thing;
	}
	public T At(int i){
		return InternalList[i];
	}
	public T Peek(){
		return InternalList[0];
	}
	public bool IsEmpty(){
		return (Count <= 0);
	}
	public void Clear(){
		InternalList.Clear();
		Count = 0;
	}

}

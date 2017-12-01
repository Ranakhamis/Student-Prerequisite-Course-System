﻿using System;

public class CourseTree
{
    //private
    ArrayList<Course> Courses;
    ArrayList<ArrayList<int>> AdjacencyList;
    int count;
    int capacity;
    void CheckIndex(params int[] indecies)
    {
        foreach (int n in indecies)
        {
            if (n < 0 || n >= AdjacencyList.Count)
            {
                throw new IndexOutOfRangeException("Course is not added to tree");
            }
        }
    }
    //public
    public CourseTree()
    {
        AdjacencyList = new ArrayList<ArrayList<int>>();
        Courses = new ArrayList<Course>();
        count = 0;
        capacity = 0;
    }
    public int Count
    {
        get => count;
    }
    public void AddCourse(Course c)
    {
        c.TreeIndex = count;
        Courses.Append(c);
        AdjacencyList.Count++;
    }
    public void Connect(Course dependant, Course dependee)
    {
        CheckIndex(dependant.TreeIndex, dependee.TreeIndex);
        if(AdjacencyList[dependant.TreeIndex] == null)
        {
            AdjacencyList[dependant.TreeIndex] = new ArrayList<int>();
        }
        AdjacencyList[dependant.TreeIndex].Append(dependee.TreeIndex);
    }
    public Course[] GetDependantCourses(Course Target, Course[] AlreadyTaken)
    {
        ArrayList<Course> res = new ArrayList<Course>();
        ArrayList<Course> queue = new ArrayList<Course>();
        queue.Append(Target);
        while(queue.Count > 0)
        {
            Course tmp = queue[Count - 1];
            queue.PopBack();
            for(int i = 0; AdjacencyList[tmp.TreeIndex] != null && i < AdjacencyList[tmp.TreeIndex].Count; i++)
            {
                Course tmpTarget = Courses[AdjacencyList[tmp.TreeIndex][i]];
                if(!Array.Exists(AlreadyTaken, new Predicate<Course>((Course a) => { return a == tmpTarget; })))
                {
                    queue.Append(tmpTarget);
                    res.Append(tmpTarget);
                }
            }
        }
        return res.ToArray();
    }
    public string[] UnloadToFile()
    {
        ArrayList<string> res = new ArrayList<string>();
        System.Text.StringBuilder tmp = new System.Text.StringBuilder();
        for(int i = 0; i < count; i++)
        {
            tmp.Append(Courses[i].Name + " ");
            if (AdjacencyList[i] == null) continue;
            for(int j = 0; j < AdjacencyList[i].Count; j++)
            {
                tmp.Append(AdjacencyList[i][j] + " ");
            }
            tmp.Remove(tmp.Length - 1, 1);
            res.Append(tmp.ToString());
            tmp.Clear();
        }
        return res.ToArray();
    }
    public void LoadFromFile(string[] filedata)
    {
        AdjacencyList.Count = filedata.Length;
        for(int i = 0; i < filedata.Length; i++)
        {
            AdjacencyList[i] = new ArrayList<int>();
            string[] fields = filedata[i].Split(' ', '\0');
            if (fields.Length <= 1) continue;
            Course c = FileOperations.CoursesFile.GetCourse(fields[0]);
            c.TreeIndex = i;
            Courses.Append(c);
            for(int j = 1; j < fields.Length; j++)
            {
                AdjacencyList[i].Append(int.Parse(fields[j]));
            }
        }
    }
}

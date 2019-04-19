using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;
using Microsoft.SqlServer.Server;

public class BitList
{
    public List<bool> ListOfBits = new List<bool>();

    public int Count => ListOfBits.Count;

    public BitList(){}
    
    public BitList(BitArray bitArray)
    {
        for (int i = 0; i < bitArray.Length; i++)
        {
            ListOfBits.Add(bitArray[i]);
        }
    }

    public BitList(int count)
    {
        ListOfBits = new List<bool>(count);
    }
    
    public BitList(List<bool> list)
    {
        ListOfBits = new List<bool>(list);
    }
    
    public BitList(BitArray bitArray,int bitCount)
    {
        for (int i = 0; i < bitArray.Length; i++)
        {
            ListOfBits.Add(bitArray[i]);
        }

        if (ListOfBits.Count < bitCount)
        {
            AddBits(bitCount-ListOfBits.Count);
        }
    }
    
    public void AddBits(int count)
    {
        for (int i = 0; i < count; i++)
        {
            ListOfBits.Add(false);
        }
    }

    public static BitList XOR(BitList owner, BitList additive)
    {
        for (int i = 0; i < additive.Count; i++)
        {
            owner.ListOfBits[i] ^= additive.ListOfBits[i];
        }
        
        return owner;
    }
    
    public static void Concate(List<bool> owner, List<bool> additive)
    {
        for (int i = 0; i < additive.Count; i++)
        {
            owner.Add(additive[i]);
        }
    }

    public static List<BitList> Split(BitList list, int countOfElements)
    {
        var result = new List<BitList>();
        
        for (int i = 0; i < list.Count; i+=countOfElements)
        {
            result.Add(new BitList(list.ListOfBits.GetRange(i,countOfElements)));
        }

        return result;
    }
    
    public static BitList RotRight(List<bool> owner, int num)
    {
        List<bool> result = new List<bool>();
        
        for (int i = owner.Count - num; i < owner.Count; i++)
        {
            result.Add(owner[i]);
        }

        for (int i = 0; i <owner.Count - num; i++)
        {
            result.Add(owner[i]);
        }
        
        //Console.WriteLine($"Number: {num} \n Owner: {printList(owner)} \n Additive: {printList(result)} \n \\\\\\\\\\\\\\\\\\");
        
        return new BitList(new List<bool>(result));
    }

    private static string printList(List<bool> list)
    {
        var result = "";
        foreach (var VARIABLE in list)
        {
            result+=VARIABLE?1:0;
        }

        return result;
    }
    
    public static BitList LogicRight(BitList owner, int num)
    {
        var a = new List<bool>(owner.ListOfBits);

        a.RemoveRange(owner.Count-1-num,num % (owner.Count-1));
        for (int i = 0; i < num; i++)
        {
            a.Insert(0,false);
        }        
        
        return new BitList(a);
    }
    
    public static BitList operator +(BitList owner, BitList additive)
    {
        if (owner.Count >= additive.Count)
        {
            for (int i = 0; i < additive.ListOfBits.Count; i++)
            {
                owner.ListOfBits[i] = owner.ListOfBits[i] || additive.ListOfBits[i];
            }
        }
        else
        {
            for (int i = 0; i < owner.ListOfBits.Count; i++)
            {
                owner.ListOfBits[i] = owner.ListOfBits[i] || additive.ListOfBits[i];
            }
        }
        return owner;
    }

    public static uint operator +(BitList owner, uint number)
    {
        var result = new List<bool>();
        var doubleNumberString = Convert.ToString(number, 2);
        var listOfDouble = new List<bool>();
        foreach (var ch in doubleNumberString)
        {
            listOfDouble.Add(ch == '1');
        }

        for (int i = 0; i < listOfDouble.Count; i++)
        {
            result.Add(owner.ListOfBits[i] | listOfDouble[i]);
        }

        var resultinNum = new List<int>();

        foreach (var bb in result)
        {
            resultinNum.Add(bb?1:0);
        }
        
        uint resultnum = Convert.ToUInt32(resultinNum.ToString(),10);

        return resultnum;
    }
    
    public static uint operator +(uint number ,BitList owner)
    {
        var result = new List<bool>();
        var doubleNumberString = Convert.ToString(number, 2);
        var listOfDouble = new List<bool>();
        foreach (var ch in doubleNumberString)
        {
            listOfDouble.Add(ch == '1');
        }

        for (int i = 0; i < listOfDouble.Count; i++)
        {
            result.Add(owner.ListOfBits[i] | listOfDouble[i]);
        }

        string resultinNum = "";

        foreach (var bb in result)
        {
            resultinNum += (bb?1:0);
        }
        
        uint resultnum = Convert.ToUInt32(resultinNum,2);

        return resultnum;
    }
    
    public static BitList operator ^(BitList owner, BitList additive)
    {
        return XOR(owner, additive);
    }
}

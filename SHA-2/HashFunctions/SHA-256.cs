using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.XPath;
using Microsoft.SqlServer.Server;

public class SHA_256_Exception : Exception
{
    public SHA_256_Exception(string message):base(message){}
}

public class SHA_256
{
    private uint[] H = 
    {
        0xC1059ED8,
        0x367CD507,
        0x3070DD17,
        0xF70E5939,
        0xFFC00B31,
        0x68581511,
        0x64F98FA7,
        0xBEFA4FA4
    };

    private uint[] K =
    {
        0x428A2F98, 0x71374491, 0xB5C0FBCF, 0xE9B5DBA5, 0x3956C25B, 0x59F111F1, 0x923F82A4, 0xAB1C5ED5,
        0xD807AA98, 0x12835B01, 0x243185BE, 0x550C7DC3, 0x72BE5D74, 0x80DEB1FE, 0x9BDC06A7, 0xC19BF174,
        0xE49B69C1, 0xEFBE4786, 0x0FC19DC6, 0x240CA1CC, 0x2DE92C6F, 0x4A7484AA, 0x5CB0A9DC, 0x76F988DA,
        0x983E5152, 0xA831C66D, 0xB00327C8, 0xBF597FC7, 0xC6E00BF3, 0xD5A79147, 0x06CA6351, 0x14292967,
        0x27B70A85, 0x2E1B2138, 0x4D2C6DFC, 0x53380D13, 0x650A7354, 0x766A0ABB, 0x81C2C92E, 0x92722C85,
        0xA2BFE8A1, 0xA81A664B, 0xC24B8B70, 0xC76C51A3, 0xD192E819, 0xD6990624, 0xF40E3585, 0x106AA070,
        0x19A4C116, 0x1E376C08, 0x2748774C, 0x34B0BCB5, 0x391C0CB3, 0x4ED8AA4A, 0x5B9CCA4F, 0x682E6FF3,
        0x748F82EE, 0x78A5636F, 0x84C87814, 0x8CC70208, 0x90BEFFFA, 0xA4506CEB, 0xBEF9A3F7, 0xC67178F2
    };
    
    public string Encrypt(string message)
    {        
        var bitList = new BitList(new BitArray(Encoding.UTF8.GetBytes(message)));
        
        if(bitList.Count>256)throw new SHA_256_Exception("Message must be equal or less then 256 bit");
        
        bitList.ListOfBits.Add(true);//Added one bit

        if (bitList.Count < 448) //Added (L + 1 + K) mod 512 zero bits
        {
            bitList.AddBits(448-bitList.Count);
        }
        
        BitList.Concate(bitList.ListOfBits,new BitList(new BitArray(message.Length),64).ListOfBits); //Added 64 bit word length

        var blocks32 = BitList.Split(bitList, 32);

        for (int i = 16; i < 63; i++)
        {
            var s0 = (BitList.RotRight(blocks32[i-15].ListOfBits, 7)) ^ (BitList.RotRight(blocks32[i-15].ListOfBits, 18)) ^ (BitList.LogicRight(blocks32[i-15],3));
            var s1 = (BitList.RotRight(blocks32[i-2].ListOfBits, 17)) ^ (BitList.RotRight(blocks32[i-2].ListOfBits, 19)) ^ (BitList.LogicRight(blocks32[i-2],10));
            blocks32.Add((blocks32[i-16] + s0) + (blocks32[i-7] + s1));
        }

        var a = H[0];
        var b = H[1];
        var c = H[2];
        var d = H[3];
        var e = H[4];
        var f = H[5];
        var g = H[6];
        var h = H[7];

        for (int i = 0; i < 63; i++)
        {
            var sum0 = (a >> 2) ^ (a >> 13) ^ (a >> 22);
            var Ma = (a & b) ^ (a & c) ^ (b & c);
            var t2 = sum0 + Ma;
            var sum1 = (e >> 6) ^ (e >> 11) ^ (e >> 25);
            var Ch = (e & f) ^ ((~e) & g);
            var t1 = h + sum1 + Ch + blocks32[i] + K[i];

            h = g;
            g = f;
            f = e;
            e = t1 + d;
            d = c;
            c = b;
            b = a;
            a = t1 + t2;
        }

        H[0] += a;
        H[1] += b;
        H[2] += c;
        H[3] += d;
        H[4] += e;
        H[5] += f;
        H[6] += g;
        H[7] += h;                      
        
        return $"{H[0]} {H[1]} {H[2]} {H[3]} {H[4]} {H[5]} {H[6]} {H[7]}";
    }

    
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


public class UDPMeneger2 : MonoBehaviour
{
    public GameObject cube;
    public Vector3 offset;
    public Vector3 rote;

    public int localPort = 5333;
    public string ipAdress = "127.0.0.1";
    public byte[] bytemessage;
    public double corX;
    public double corY;
    public double corZ;
    public double anglePitch;
    public double angleRoll;
    public double angleYaw;
    public double V;
    public double Drv;
    public double De;
    public double Dn;
    public double Dz;
    public double Di;
    public double Dp;
    public double RUD;

    Thread thread;
    static UdpClient client;

    void Start()
    {
        //начальные координаты
        corX = 0;
        corY = 100;
        corZ = 0;
        V = 0;

        //поток передачи данных
        thread = new Thread(new ThreadStart(ReceiveData));
        thread.Start();
    }

    void Update()
    {
        //координаты
        offset.x = (float)corZ;
        offset.y = (float)corY;
        offset.z = (float)corX;
        transform.position = offset;

        //углы поворота
        rote.x = -(float)anglePitch;
        rote.y = (float)angleYaw;
        rote.z = (float)angleRoll;

        transform.rotation = Quaternion.Euler(rote); 
    }

    private void ReceiveData()
    {
        client = new UdpClient(5333); 

        while (true)
        {
            try
            {
                IPEndPoint senderIP = new IPEndPoint(IPAddress.Parse(ipAdress), localPort);
                byte[] data = client.Receive(ref senderIP);
                
                corX = BitConverter.ToDouble(data,0);
                corY = BitConverter.ToDouble(data,8);
                corZ = BitConverter.ToDouble(data,16);
                anglePitch = BitConverter.ToDouble(data,24);
                angleRoll = BitConverter.ToDouble(data,32);
                angleYaw = BitConverter.ToDouble(data,40);
                V = BitConverter.ToDouble(data,64);
                Drv = BitConverter.ToDouble(data,72);
                De = BitConverter.ToDouble(data,80);
                Dn = BitConverter.ToDouble(data,88);
                Dz = BitConverter.ToDouble(data,96);
                Di = BitConverter.ToDouble(data,104);
                Dp = BitConverter.ToDouble(data,112);
                RUD = BitConverter.ToDouble(data,120); 

                //Debug.Log(data);
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }
}


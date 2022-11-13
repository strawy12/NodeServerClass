using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.WebSockets;
using UnityEditor;
using System;
using System.Threading;
using System.Security.Cryptography;
using GGM.Proto.Tank;
using Google.Protobuf;

public class SocketManager : MonoBehaviour
{
    private ClientWebSocket _socket = null;

    // ��Ʋ �����
    private void Start()
    {
        Connection();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            CMove cmove = new CMove { PlayerId = 1 , X = 20f , Y = 30f };
            ushort len = (ushort)(cmove.CalculateSize() + 4);

            ArraySegment<byte> segment = new ArraySegment<byte>(new byte[len]);
            Array.Copy(BitConverter.GetBytes(len), 0, segment.Array, segment.Offset, sizeof(ushort));
            Array.Copy(BitConverter.GetBytes((ushort)MSGID.Cmove), 0, segment.Array, segment.Offset + 2, sizeof(ushort));
            Array.Copy(cmove.ToByteArray(), 0, segment.Array, segment.Offset + 4, len - 4);

            SendData(segment);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
        }
    }

    public async void Connection()
    {
        Debug.Log("Ŀ�ؼ� ����");
        if (_socket != null && _socket.State == WebSocketState.Open)
        {
            Debug.Log(" �̹� ���� �Ǿ� �մ� �����Դϴ�");
            return;
        }

        _socket = new ClientWebSocket();
        Uri serverUri = new Uri("ws://localhost:50000");

        await _socket.ConnectAsync(serverUri, CancellationToken.None);

        Debug.Log("���� �Ϸ�");

        //ArraySegment<byte> bufferSegment = new ArraySegment<byte>(new byte[1024]);
        //WebSocketReceiveResult result = await _socket.ReceiveAsync(bufferSegment, CancellationToken.None);

        //string msg = System.Text.Encoding.UTF8.GetString(bufferSegment.Array);
        //Debug.Log(msg);
    }

    public async void SendData(ArraySegment<byte> segment)
    {
        await _socket.SendAsync(segment, WebSocketMessageType.Binary, true, CancellationToken.None);
    }

    private List<string> msg = new List<string>();

    async void ReceiveLoop()
    {

    }

    private void OnDestroy()
    {
        Disconnect();
    }

    public void Disconnect()
    {
        if (_socket != null)
        {
            _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Quit", CancellationToken.None);
        }
    }
}

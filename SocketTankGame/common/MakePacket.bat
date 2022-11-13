protoc -I=.\ --ts_out=..\socketServer\src\packet packet.proto

protoc -I=.\ --csharp_out=..\TankClient\Assets\01.Scripts\Packet packet.proto

pause
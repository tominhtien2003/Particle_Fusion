using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;

public class NetworkRunnerHandler : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner _runner;
    [SerializeField] NetworkPrefabRef playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    private GameInput gameInput;

    async void StartGame(GameMode mode)
    {
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }
        else
        {
            Debug.LogWarning("Scene isn't valid");
        }
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });
    }
    private void Start()
    {
        StartGame(GameMode.AutoHostOrClient);
    }
    private void Update()
    {
        
    }
    // Được gọi khi một đối tượng rời khỏi vùng quan sát (Area of Interest) của người chơi.
    // 'runner' là đối tượng NetworkRunner đang điều hành phiên.
    // 'obj' là đối tượng mạng đã rời khỏi vùng quan sát.
    // 'player' là người chơi liên quan.
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    // Được gọi khi một đối tượng mới vào vùng quan sát (Area of Interest) của người chơi.
    // 'runner' là đối tượng NetworkRunner đang điều hành phiên.
    // 'obj' là đối tượng mạng vừa vào vùng quan sát.
    // 'player' là người chơi liên quan.
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    // Được gọi khi một người chơi mới tham gia vào phiên.
    // 'runner' là đối tượng NetworkRunner đang điều hành phiên.
    // 'player' là người chơi mới tham gia.
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);
            NetworkObject networkPlayerObject = runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);

            spawnedCharacters.Add(player, networkPlayerObject);
        }
    }

    // Được gọi khi một người chơi rời khỏi phiên.
    // 'runner' là đối tượng NetworkRunner đang điều hành phiên.
    // 'player' là người chơi đã rời khỏi.
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            spawnedCharacters.Remove(player);
        }
    }

    // Được gọi khi cần lấy dữ liệu đầu vào từ người chơi.
    // 'runner' là đối tượng NetworkRunner đang điều hành phiên.
    // 'input' chứa dữ liệu đầu vào từ người chơi.
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();

        if (PlayerController.LocalPlayerInstance != null)
        {
            gameInput = PlayerController.LocalPlayerInstance.GetComponent<GameInput>();
        }

        if (gameInput != null)
        {
            data.direction = gameInput.GetMoveDirectionPlayer();
        }
        else
        {
            Debug.Log("gameInput is null");
        }
        input.Set(data);
    }

    // Được gọi khi dữ liệu đầu vào từ người chơi bị thiếu hoặc không hợp lệ.
    // 'runner' là đối tượng NetworkRunner đang điều hành phiên.
    // 'player' là người chơi có dữ liệu đầu vào bị thiếu.
    // 'input' là dữ liệu đầu vào không hợp lệ hoặc bị thiếu.
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }

    // Được gọi khi NetworkRunner bị tắt hoặc ngừng hoạt động.
    // 'runner' là đối tượng NetworkRunner đang điều hành phiên.
    // 'shutdownReason' là lý do NetworkRunner bị tắt.
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    // Được gọi khi kết nối đến máy chủ thành công.
    // 'runner' là đối tượng NetworkRunner đang điều hành phiên.
    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("OnConnectedToServer");
    }

    // Được gọi khi kết nối đến máy chủ bị mất hoặc bị lỗi.
    // 'runner' là đối tượng NetworkRunner đang điều hành phiên.
    // 'reason' là lý do mất kết nối.
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {

    }

    // Được gọi khi có yêu cầu kết nối từ một người chơi hoặc máy khách.
    // 'runner' là đối tượng NetworkRunner đang điều hành phiên.
    // 'request' chứa thông tin yêu cầu kết nối.
    // 'token' là token dùng để xác thực kết nối.
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {

    }

    // Được gọi khi kết nối đến máy chủ không thành công.
    // 'runner' là đối tượng NetworkRunner đang điều hành phiên.
    // 'remoteAddress' là địa chỉ máy chủ từ xa.
    // 'reason' là lý do kết nối thất bại.
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {

    }

    // Được gọi khi có tin nhắn mô phỏng từ người chơi.
    // 'runner' là đối tượng NetworkRunner đang điều hành phiên.
    // 'message' chứa dữ liệu tin nhắn mô phỏng.
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }

    // Được gọi khi danh sách các phiên mạng được cập nhật.
    // 'runner' là đối tượng NetworkRunner đang điều hành phiên.
    // 'sessionList' chứa danh sách các phiên mạng hiện tại.
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {

    }

    // Được gọi khi nhận được phản hồi từ xác thực tùy chỉnh.
    // 'runner' là đối tượng NetworkRunner đang điều hành phiên.
    // 'data' chứa dữ liệu phản hồi từ xác thực tùy chỉnh.
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {

    }

    // Được gọi khi xảy ra chuyển giao chủ (host migration) trong phiên.
    // 'runner' là đối tượng NetworkRunner đang điều hành phiên.
    // 'hostMigrationToken' chứa thông tin về chuyển giao chủ.
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {

    }

    // Được gọi khi nhận được dữ liệu đáng tin cậy từ người chơi.
    // 'runner' là đối tượng NetworkRunner đang điều hành phiên.
    // 'player' là người chơi gửi dữ liệu.
    // 'key' là khóa để nhận diện dữ liệu.
    // 'data' là dữ liệu nhận được.
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {

    }

    // Được gọi khi dữ liệu đáng tin cậy đang được truyền và tiến trình của nó được cập nhật.
    // 'runner' là đối tượng NetworkRunner đang điều hành phiên.
    // 'player' là người chơi gửi dữ liệu.
    // 'key' là khóa để nhận diện dữ liệu.
    // 'progress' là tỷ lệ tiến trình gửi dữ liệu (0.0 đến 1.0).
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {

    }

    // Được gọi khi việc tải cảnh hoàn tất.
    // 'runner' là đối tượng NetworkRunner đang điều hành phiên.
    public void OnSceneLoadDone(NetworkRunner runner)
    {

    }

    // Được gọi khi việc tải cảnh bắt đầu.
    // 'runner' là đối tượng NetworkRunner đang điều hành phiên.
    public void OnSceneLoadStart(NetworkRunner runner)
    {

    }
}

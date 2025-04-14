using Bas.Pennings.DevTools;
using UnityEngine;

/// <summary>
/// Manages game resets, game starts and game ends. Can be referenced using "GameManager.Instance".
/// </summary>
public class GameManager : AbstractSingleton<GameManager>
{
    [Header("References")]
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _layerBodyPrefab;
    [SerializeField] private GameObject _playerObjectsParent;
    [SerializeField] private Transform _playerSpawnPoint;
    private GameObject _playerObject;

    public void SetPlayerSpawnPoint(Transform spawnPoint)
        => _playerSpawnPoint = spawnPoint;

    /// <summary>
    /// Destroys the player, spawns a body at the last player location and spawns a new player at the player spawn point;
    /// </summary>
    public void ResetLevel()
    {
        Destroy(_playerObject);
        InstantiateBody();
        _playerObject = InstantiatePlayer();
    }

    private void Start()
        => _playerObject = InstantiatePlayer();

    /// <summary>
    /// Instantiates a player at the player spawn point.
    /// </summary>
    /// <returns>The instantiated player</returns>
    private GameObject InstantiatePlayer()
        => Instantiate(_playerPrefab, _playerSpawnPoint);

    /// <summary>
    /// Instantiates a body at the player position. 
    /// </summary>
    private void InstantiateBody()
        => Instantiate(_layerBodyPrefab, _playerObject.transform);

    /// <summary>
    /// Instantiates a game object under the player objects' parent.
    /// </summary>
    /// <param name="gameObject">The game object to instantiate</param>
    /// <param name="transform">The transform at which to spawn the game object</param>
    /// <returns></returns>
    private GameObject Instantiate(GameObject gameObject, Transform transform)
        => Instantiate(
            gameObject,
            transform.position,
            transform.rotation,
            _playerObjectsParent.transform);
}

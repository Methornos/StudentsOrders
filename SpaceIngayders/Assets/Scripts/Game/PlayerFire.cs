using UnityEngine;
using Photon.Pun;

public class PlayerFire : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _firePosition = default;
    [SerializeField]
    private GameObject _bulletPrefab = default;

    private PhotonView _view = default;

    public float AttackRate = 0.5f;
    public KeyCode FireKey = KeyCode.K;

    private void Start()
    {
        _view = gameObject.GetPhotonView();
    }

    private void Update()
    {
        if (!_view.IsMine) return;

        if (Input.GetKeyDown(FireKey))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        PhotonNetwork.Instantiate(_bulletPrefab.name, _firePosition.position, Quaternion.identity);
    }
}

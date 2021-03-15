using UnityEngine;
using Photon.Pun;

public class PlayerHealth : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _healthbar = default;
	
	private PhotonView _view = default;
    private PhotonView _manager = default;

    public int Health = 10;

	private void Start()
	{
		_view = gameObject.GetPhotonView();
        _manager = GameObject.Find("GameManager").GetPhotonView();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Meteor")
        {
            ApplyDamage(2);
        }
        if(collision.transform.tag == "field")
        {
            ApplyDamage(1);
        }
    }

    private void ApplyDamage(int damage)
    {
        int current = Health;

        if (current > damage)
        {
            current -= damage;
            Health = current;

            _healthbar.localScale = new Vector2(Health, 1);
        }
        else
        {
            Destroy();
        }
    }

    private void Destroy()
    {
		if(_view.IsMine)
		{
            _manager.RPC("KillPlayer", RpcTarget.All);
			PhotonNetwork.Destroy(gameObject);
		}
    }
}

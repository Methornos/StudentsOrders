using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPunCallbacks
{
    private Rigidbody2D _self = default;
	
	private PhotonView _view = default;
	
    private void Start()
    {
		_view = gameObject.GetPhotonView();
        _self = GetComponent<Rigidbody2D>();
        _self.velocity = new Vector2(0, 20);

        Destroy(gameObject, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Meteor")
        {
            Destroy();
        }
    }

    private void Destroy()
    {
		if(_view.IsMine)
		{
			PhotonNetwork.Destroy(gameObject);
		}
    }
}

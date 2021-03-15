using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviourPunCallbacks
{
    private Transform _self = default;
    private Rigidbody2D _rigidbody = default;

    private PhotonView _view = default;

    public float Speed = default;

    private void Start()
    {
        _self = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _view = gameObject.GetPhotonView();
    }

    private void Update()
    {
        if (!_view.IsMine) return;

        if(Input.GetKey(KeyCode.A))
        {
            _rigidbody.AddForce(Vector2.left * Speed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            _rigidbody.AddForce(-Vector2.left * Speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "field")
        {
            if(collision.transform.name == "field_left")
            {
                _self.position = new Vector2(_self.position.x + 2, -3);
                _rigidbody.velocity = new Vector2(0, 0);
            }
            else
            {
                _self.position = new Vector2(_self.position.x - 2, -3);
                _rigidbody.velocity = new Vector2(0, 0);
            }
        }
    }
}

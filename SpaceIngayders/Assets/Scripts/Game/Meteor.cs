using System.Collections;
using UnityEngine;
using Photon.Pun;

public class Meteor : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TextMesh _healthText = default;

    private Scores _scores = default;
	
	private PhotonView _view = default;

    private Rigidbody2D _self = default;
    private SpriteRenderer _render = default;
    private ParticleSystem _destroyParticle = default;

    private float _rotationAngle = 0;

    public int Health = 1;

    private void Start()
    {
        _self = GetComponent<Rigidbody2D>();
        _render = GetComponent<SpriteRenderer>();
        _destroyParticle = GameObject.Find("DestroyParticle").GetComponent<ParticleSystem>();
        _scores = GameObject.Find("Scores").GetComponent<Scores>();
		_view = gameObject.GetPhotonView();

        _rotationAngle = Random.Range(-3f, 3f);

        int multiplier = Random.Range(1, 6);
        float scale = multiplier / 10f;

        Health = multiplier;
       
        transform.localScale = new Vector3(scale, scale, 1);

        _healthText.text = Health.ToString();

        FallSide();
    }

    private void Update()
    {
        transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z + _rotationAngle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Bullet")
        {
            ApplyDamage(1);
        }
        if(collision.transform.tag == "Player")
        {
            Destroy();
        }
    }

    private void ApplyDamage(int damage)
    {
        int current = Health;

        if (current > damage)
        {
            current -= damage;
            Health = current;
            _healthText.text = Health.ToString();

            StartCoroutine(TakeDamage());
        }
        else
        {
            Destroy();
        }
    }

    private void FallSide()
    {
        int x = default;
        int y = Random.Range(-3, 0);

        if(transform.position.x >= 0)
        {
            x = Random.Range(-3, -1);
        }
        else
        {
            x = Random.Range(1, 3);
        }

        _self.velocity = new Vector2(x, y);
    }

    private IEnumerator TakeDamage()
    {
        _render.color = new Color(0.5f, 0.5f, 0.5f);

        yield return new WaitForSeconds(0.1f);

        _render.color = new Color(1, 1, 1);
    }

    private void Destroy()
    {
		_scores.RaiseScore(1);

		_destroyParticle.transform.position = transform.position;
		_destroyParticle.Play();
		
		if(_view.IsMine)
		{
			PhotonNetwork.Destroy(gameObject);
		}
    }
}

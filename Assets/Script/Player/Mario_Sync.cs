using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Mario_Sync : MonoBehaviourPun, IPunObservable
{

    Vector3 _networkPosition;
    Quaternion _networkRotation;
    Rigidbody2D _rb;
    PhotonView PV;
    SpriteRenderer SR;

	private void Awake()
	{
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;

        SR = gameObject.GetComponent<SpriteRenderer>();
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        //if (stream.IsWriting)
        //{
        //    stream.SendNext(transform.position);
        //    stream.SendNext(transform.rotation);
        //}
        //else if (stream.IsReading)
        //{
        //    _networkPosition = (Vector3)stream.ReceiveNext();
        //    _networkRotation = (Quaternion)stream.ReceiveNext();

        //}

        //if (stream.IsWriting)
        //{
        //    stream.SendNext(_rb.position);
        //    stream.SendNext(_rb.rotation);
        //    stream.SendNext(_rb.velocity);
        //}
        //else
        //{
        //    _networkPosition = (Vector3)stream.ReceiveNext();
        //    _networkRotation = (Quaternion)stream.ReceiveNext();
        //    _rb.velocity = (Vector3)stream.ReceiveNext();

        //    float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTimestamp));
        //    _networkPosition += new Vector3(_rb.velocity.x, _rb.velocity.y, 0) * lag;
        //}


    }

    

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        PV = GetComponent<PhotonView>();
    }

    public void FixedUpdate()
    {
        if (!PV.IsMine)
        {
            //Debug.Log(_rb.position + " 에서 " + _networkPosition + " 로 가는중..");
            //_rb.position = Vector3.MoveTowards(_rb.position, _networkPosition, Time.fixedDeltaTime);
            //_rb.rotation = Quaternion.RotateTowards(_rb.rotation, _networkRotation, Time.fixedDeltaTime * 100.0f);
        }
    }
}

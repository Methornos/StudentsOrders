using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class BrushColor : MonoBehaviourPunCallbacks, IPointerDownHandler
{
    private PhotonView _drawingView = default;

    public int ColorId = 0;

    private void Awake()
    {
        _drawingView = GameObject.Find("DrawZone").GetPhotonView();
    }

    //по нажатию на цветные кнопочки у каждого игрока устанавливается цвет кисти, чтобы создавалась одинаковая линия
    public void OnPointerDown(PointerEventData eventData)
    {
        _drawingView.RPC("ChangeColor", RpcTarget.All, ColorId);
    }
}

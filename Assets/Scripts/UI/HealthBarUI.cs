using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private PlayerStatus _player;
    [SerializeField] private Image _relleno;       // la imagen que se achica
    [SerializeField] private TMP_Text _textoVidas; // opcional

    [SerializeField] private Color _colorNormal = Color.green;
    [SerializeField] private Color _colorBajo = Color.red;
    [SerializeField] private float _velocidadBarra = 5f;

    private float _valorMostrado = 1f;

    void Start()
    {
        if (_player == null) _player = FindFirstObjectByType<PlayerStatus>();
    }

    void Update()
    {
        if (_player == null || _relleno == null) return;

        float objetivo = 0f;
        if (_player.VidaMaxima > 0f) objetivo = _player.VidaActual / _player.VidaMaxima;

        //la barra baja suave en vez de saltar
        _valorMostrado = Mathf.Lerp(_valorMostrado, objetivo, _velocidadBarra * Time.unscaledDeltaTime);

        //achico el relleno en X (por eso el pivot tiene que estar a la izquierda)
        _relleno.rectTransform.localScale = new Vector3(_valorMostrado, 1f, 1f);

        if (_player.Estado == PlayerStatus.playerState.LowHealth) _relleno.color = _colorBajo;
        else _relleno.color = _colorNormal;

        if (_textoVidas != null) _textoVidas.text = "Vidas: " + _player.Vidas;
    }
}
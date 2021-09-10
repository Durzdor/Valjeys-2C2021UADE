using UnityEngine;
using TMPro;


namespace Assets.Scripts.Enemies.DamagePopup
{
    public class DamagePopup : MonoBehaviour
    {
        private static int _sortingOrder;

        private const float DISAPPEAR_TIMER_MAX = 1f;

        private Camera _camera;
        private float _disappearTimer;
        private Color _textColor;
        private Vector3 _moveVector;
        private TextMeshPro _tm;


        private void Awake()
        {
            _tm = GetComponent<TextMeshPro>();
            _moveVector = new Vector3(0, 1);
            _disappearTimer = DISAPPEAR_TIMER_MAX;
            _camera = Camera.main;
        }

        public void ShowDamage(Vector3 position, int amount)
        {
            transform.position = position;
            _tm.SetText(amount.ToString());
            _tm.fontSize = 7;
            _textColor = Color.yellow;
        }

        private void Update()
        {
            transform.LookAt(_camera.transform);
            transform.forward *= -1;
            transform.position += _moveVector * Time.deltaTime;
            _moveVector -= _moveVector * 8f * Time.deltaTime;

            if (_disappearTimer > DISAPPEAR_TIMER_MAX * .5f)
            {
                // First half of the popup lifetime
                float increaseScaleAmount = 1f;
                transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
            }
            else
            {
                // Second half of the popup lifetime
                float decreaseScaleAmount = 1f;
                transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
            }

            _disappearTimer -= Time.deltaTime;
            if (_disappearTimer < 0)
            {
                // Start disappearing
                float disappearSpeed = 3f;
                _textColor.a -= disappearSpeed * Time.deltaTime;
                _tm.color = _textColor;
                if (_textColor.a < 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}

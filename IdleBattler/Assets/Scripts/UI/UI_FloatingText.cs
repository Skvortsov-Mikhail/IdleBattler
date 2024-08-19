using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_FloatingText : MonoBehaviour
{
    private float _duration;
    private float _textScaleFactor;
    private float _textLiftingDistance;
    private float _textOpacityValue;

    private TMP_Text _damageText;

    private Timer _timer;

    private void Update()
    {
        if (_timer != null)
        {
            _timer.RemoveTime(Time.deltaTime);

            if (_timer.IsFinished)
            {
                Destroy(gameObject);
            }
        }
    }

    public void InitFloatingText(string text, Color textColor, bool isAnimated, float lifeTime, float textScaleFactor = 1, float textLiftingDistance = 0, float textOpacityValue = 1)
    {
        _damageText = GetComponentInChildren<TMP_Text>();

        _damageText.text = text;
        _damageText.color = textColor;

        _duration = lifeTime;
        _textScaleFactor = textScaleFactor;
        _textLiftingDistance = textLiftingDistance;
        _textOpacityValue = textOpacityValue;

        if (isAnimated == true)
        {
            AnimateText();
        }
        else
        {
            _timer = new Timer(lifeTime);
        }
    }

    private void AnimateText()
    {
        _damageText.transform.DOScale(_damageText.transform.localScale * _textScaleFactor, _duration);
        _damageText.transform.DOLocalMoveY(_textLiftingDistance, _duration);
        _damageText.DOFade(_textOpacityValue, _duration).OnComplete(() => { Destroy(gameObject); });
    }
}
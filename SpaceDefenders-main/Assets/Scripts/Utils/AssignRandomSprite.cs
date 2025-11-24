using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AssignRandomSprite : MonoBehaviour
{
    [System.Serializable]
    public struct SpritePair
    {
        public Sprite normal;
        public Sprite alternate;
    }

    [SerializeField] private SpritePair[] spritePairs;
    private SpritePair _chosenPair;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        ChooseRandomPair();
        SetAlternate(false);
    }

    private void ChooseRandomPair()
    {
        if (spritePairs == null || spritePairs.Length == 0)
        {
            return;
        }
        int index = Random.Range(0, spritePairs.Length);
        _chosenPair = spritePairs[index];
    }

    public void SetAlternate(bool useAlternate)
    {
        if (_chosenPair.normal == null)
        {
            return;
        }

        if (useAlternate && _chosenPair.alternate != null)
        {
            _spriteRenderer.sprite = _chosenPair.alternate;
        }
        else
        {
            _spriteRenderer.sprite = _chosenPair.normal;
        }
    }
}

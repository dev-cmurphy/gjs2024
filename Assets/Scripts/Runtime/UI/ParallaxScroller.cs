using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kc.runtime
{
    public class ParallaxScroller : MonoBehaviour
    {
        [SerializeField]
        List<Transform> _pairedTransforms;

        List<SpriteRenderer> _renderers;

        // Use this for initialization
        void Awake()
        {
            _renderers = new List<SpriteRenderer>();
            for(int i = 0; i < _pairedTransforms.Count; i++)
            {
                _renderers.Add(_pairedTransforms[i].GetComponent<SpriteRenderer>());
            }
        }

        // Update is called once per frame
        void Update()
        {
            for(int i = 0; i < _renderers.Count; i++)
            {
                MaterialPropertyBlock block = new MaterialPropertyBlock();
                _renderers[i].GetPropertyBlock(block);
                block.SetVector("_Offset", new Vector2(transform.position.x, transform.position.y));
                block.SetFloat("_ParallaxDepth", Mathf.Abs(10f / _pairedTransforms[i].position.z));
                _renderers[i].SetPropertyBlock(block);
            }
        }
    }
}
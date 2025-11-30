using UnityEngine;
using System.Collections.Generic;

namespace Platformer.Mechanics
{
    public class TokenController : MonoBehaviour
    {
        [Tooltip("Frames per second at which tokens are animated.")]
        public float frameRate = 12;

        [Tooltip("Token instances. If empty, they will be auto-detected by tag 'Token'.")]
        public List<TokenInstance> tokens = new List<TokenInstance>();

        float nextFrameTime = 0;

        void Awake()
        {
            // Si la lista está vacía → buscar tokens automáticamente
            if (tokens.Count == 0)
            {
                GameObject[] found = GameObject.FindGameObjectsWithTag("Token");
                Debug.Log("🔍 Tokens encontrados con Tag 'Token': " + found.Length);

                foreach (var obj in found)
                {
                    TokenInstance t = obj.GetComponent<TokenInstance>();
                    if (t != null)
                    {
                        tokens.Add(t);
                    }
                }

                Debug.Log("🎯 Tokens detectados automáticamente: " + tokens.Count);
            }

            // Registrar índices y controller
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i] != null)
                {
                    tokens[i].tokenIndex = i;
                    tokens[i].controller = this;
                }
            }
        }

        void Update()
        {
            if (Time.time - nextFrameTime > (1f / frameRate))
            {
                for (int i = 0; i < tokens.Count; i++)
                {
                    var token = tokens[i];

                    if (token != null)
                    {
                        token._renderer.sprite = token.sprites[token.frame];

                        if (token.collected && token.frame == token.sprites.Length - 1)
                        {
                            token.gameObject.SetActive(false);
                            tokens[i] = null;
                        }
                        else
                        {
                            token.frame = (token.frame + 1) % token.sprites.Length;
                        }
                    }
                }

                nextFrameTime += 1f / frameRate;
            }
        }
    }
}

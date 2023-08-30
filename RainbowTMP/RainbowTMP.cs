using System.Collections;
using TMPro;
using UnityEngine;
namespace FourteenDynamics.TMPExtentions.Rainbow {
    public class RainbowTMP : MonoBehaviour {
        [SerializeField] private bool _useCustomGradient;
        [SerializeField] private bool _detailedGradient;
        [SerializeField] private Gradient _thisGradient;

        [SerializeField] private Gradient _bottomLeft;
        [SerializeField] private Gradient _topRight;
        [SerializeField] private Gradient _bottomRight;

        [SerializeField] private bool _individualCharacters = false;
        [SerializeField] [Tooltip("Start Value of the first selected Color. \n -0.01 results in a random value.")] [Range(-0.01f, 1f)] private float _startValue = 0f;

        [SerializeField] [Range(0.001f, 20f)] private float _duration;
        private float _curTime = 0;

        [SerializeField] private bool _ignoreSpaces = true;
        [SerializeField][Range(0.001f, 1f)] private float _differenceCharacters = 0.01f;
        [SerializeField][Range(0.001f, 60f)] private float _updatesPerSecond = 10;

        private TMP_Text _t;

        void Start() {
            _t = GetComponent<TMP_Text>();
            _t.ForceMeshUpdate();

            if(_startValue < 0) {
                _startValue = Random.value;
            }
            _curTime = _startValue;
            _firstDiff = _startValue;

            if(!_useCustomGradient) {
                _thisGradient = Utils.GetRainbow();
            }
            if(_individualCharacters) {
                StartCoroutine(RainbowCharacters());
            }
        }

        void Update() {
            if(!_individualCharacters) {
                _curTime += Time.deltaTime;
                _curTime %= _duration;
                _t.color = _thisGradient.Evaluate(_curTime / _duration);
            }
        }

        private float _firstDiff = 0;
        private TMP_TextInfo _i;
        private IEnumerator RainbowCharacters() {
            while(true) {
                _i = _t.textInfo;
                int length = _t.text.Length;
                float diff = _firstDiff;
                for(int i = 0; i < length; i++) {
                    if(_i.characterInfo[i].character == ' ' && _ignoreSpaces) {
                        continue;
                    }

                    int meshIndex = _i.characterInfo[i].materialReferenceIndex;
                    int vertexIndex = _i.characterInfo[i].vertexIndex;
                    Color32[] vertexColors = _i.meshInfo[meshIndex].colors32;

                    Color tL = _thisGradient.Evaluate(diff);
                    Color32 tLC =
                        new((byte) (tL.r * 255), (byte) (tL.g * 255), (byte) (tL.b * 255), vertexColors[vertexIndex + 1].a);
                    if(_detailedGradient) {
                        Color bL = _bottomLeft.Evaluate(diff);
                        Color32 bLC =
                            new((byte) (bL.r * 255), (byte) (bL.g * 255), (byte) (bL.b * 255), vertexColors[vertexIndex + 0].a);

                        Color bR = _bottomRight.Evaluate(diff);
                        Color32 bRC =
                            new((byte) (bR.r * 255), (byte) (bR.g * 255), (byte) (bR.b * 255), vertexColors[vertexIndex + 3].a);

                        Color tR = _topRight.Evaluate(diff);
                        Color32 tRC =
                            new((byte) (tR.r * 255), (byte) (tR.g * 255), (byte) (tR.b * 255), vertexColors[vertexIndex + 2].a);

                        vertexColors[vertexIndex + 1] = tLC; // top left
                        vertexColors[vertexIndex + 2] = tRC; // top right
                        vertexColors[vertexIndex + 0] = bLC; // bottom left
                        vertexColors[vertexIndex + 3] = bRC; // bottom right
                    } else {
                        vertexColors[vertexIndex + 1] = tLC; // top left
                        vertexColors[vertexIndex + 2] = tLC; // top right
                        vertexColors[vertexIndex + 0] = tLC; // bottom left
                        vertexColors[vertexIndex + 3] = tLC; // bottom right
                    }
                    

                    diff += _differenceCharacters;
                    diff %= 1;
                }
                _firstDiff += _differenceCharacters;
                _firstDiff %= 1;

                _t.UpdateVertexData(TMP_VertexDataUpdateFlags.All);

                yield return new WaitForSeconds(1 / _updatesPerSecond);
            }


        }

        public void SetGradientsEqual() {
            GradientColorKey[] gck = _thisGradient.colorKeys;
            GradientAlphaKey[] ack = _thisGradient.alphaKeys;

            _bottomLeft = new Gradient {
                colorKeys = gck,
                alphaKeys = ack
            };
            _bottomRight = new Gradient {
                colorKeys = gck,
                alphaKeys = ack
            };
            _topRight = new Gradient {
                colorKeys = gck,
                alphaKeys = ack
            };
        }


    }
    public static class Utils {
        public static Gradient GetRainbow() {
            Gradient r = new();
            GradientColorKey[] gck = {
                new(new(255/255f,       0f,       0f), 0),
                new(new(255/255f, 165/255f,       0f), 0.125f),
                new(new(255/255f, 255/255f,       0f),  0.25f),
                new(new(      0f, 255/255f,       0f), 0.375f),
                new(new(      0f,       0f, 255/255f), 0.500f),
                new(new( 75/255f,       0f, 130/255f), 0.625f),
                new(new(238/255f, 130/255f, 238/255f), 0.750f),
                new(new(255/255f,       0f,       0f), 1),
            };
            GradientAlphaKey[] al = {
                new(1,0)
            };

            r.SetKeys(gck, al);
            return r;
        }
    }
}
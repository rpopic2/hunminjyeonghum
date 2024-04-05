using TMPro;
using UnityEngine;

public class Audio : MonoBehaviour
{
    const int sampleRate = 44100;
    const float frequency = 440.0f;

    [SerializeField] float dutyCycle = 0.5f;
    [SerializeField] TMP_Text _text;
    float phase;
    float angularFrequency;

    bool isFirstOn;
    bool isMiddleOn;
    bool isLastOn;

    void Start()
    {
        _text.text = new string(new []{'\u1118', '\u116A', '\u11AE'});
        angularFrequency = 2 * Mathf.PI * frequency / sampleRate;
    }

    void Update()
    {
        isFirstOn = Input.GetKey("d");
        isMiddleOn = Input.GetKey(".");
        isLastOn = Input.GetKey("f");
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += channels)
        {
            float square1 = 0;
            if (isFirstOn) {
                square1 = phase < Mathf.PI  * dutyCycle ? 1f : -1f;
                square1 *= 0.3f;
            }

            float square2 = 0;
            if (isLastOn) {
                square2 = phase < Mathf.PI ? 1f : -1f;
                square2 *= 0.3f;
            }
            float tri = 0;
            if (isMiddleOn) {
                tri = Mathf.PingPong(phase, 2f) - 1f;
                tri *= 0.3f;
            }


            for (int channel = 0; channel < channels; channel++)
            {
                data[i + channel] = square1 + tri + square2;
            }

            phase += angularFrequency;

            if (phase > 2 * Mathf.PI)
            {
                phase -= 2 * Mathf.PI;
            }
        }
    }
}

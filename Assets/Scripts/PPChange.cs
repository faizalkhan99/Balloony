using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PPChange : MonoBehaviour
{

    [SerializeField] private PostProcessVolume _postProcessingVolume;
    [SerializeField] private PostProcessProfile[] _postProcessProfiles;

    private int _postProcessingProfileIndex;
    [SerializeField] private int _transitionTime;

    void Start()
    {
        _postProcessingProfileIndex = 0;
        _postProcessingVolume = GetComponent<PostProcessVolume>();
        _postProcessingVolume.profile = _postProcessProfiles[_postProcessingProfileIndex];

        StartCoroutine(ChangeProfileOverTime());
    }

    private IEnumerator ChangeProfileOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(_transitionTime);

            _postProcessingProfileIndex++;
            if (_postProcessingProfileIndex >= _postProcessProfiles.Length)
            {
                _postProcessingProfileIndex = 0;
            }

            _postProcessingVolume.profile = _postProcessProfiles[_postProcessingProfileIndex];
            
        }
    }
}
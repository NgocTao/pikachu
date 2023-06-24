using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ITutorialDirector
{
    void HideHighlightObjects();
    void HighlightObjectIndex(int index);
    void RunCoroutine(IEnumerator coroutine);
    void FinishTutorial();
}

[System.Serializable]
public class TutorialData
{
    public List<Vector3> Positions;
    public List<int> Steps;
}

public class TutorialStep
{
    TutorialData _data;

    ITutorialDirector _director;

    int _step = 0;

    public TutorialStep(ITutorialDirector director, string text)
    {
        _director = director;
        _data = Newtonsoft.Json.JsonConvert.DeserializeObject<TutorialData>(text);
        _step = 0;
    }

    public void Start()
    {
        _director.RunCoroutine(Play());
    }

    public IEnumerator Play()
    {
        yield return null;
        _director.HighlightObjectIndex(_step);
    }

    public Vector3 GetPositionByIndex(int index)
    {
        return _data.Positions[index];
    }

    public void FinishStep()
    {
        _director.HideHighlightObjects();
        _step++;
        if (_step < _data.Steps.Count)
            _director.RunCoroutine(Play());
        else
            _director.FinishTutorial();
    }
}

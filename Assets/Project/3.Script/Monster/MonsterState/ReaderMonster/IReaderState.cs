using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReaderState
{
    void StartState(ReaderMonsAI r_Monster);

    void UpdateState(ReaderMonsAI r_Monster);

    void ExitState(ReaderMonsAI r_Monster);

}
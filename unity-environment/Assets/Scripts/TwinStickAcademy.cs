using System.Collections.Generic;
using MLAgents;
using UnityEngine;

public class TwinStickAcademy : Academy
{
    public int ArenaCount = 6;

    [SerializeField]
    private GameObject _arenaPrefab;

    [SerializeField]
    public Brain _movementBrain;

    [SerializeField]
    public Brain _aimBrain;

    private List<Arena> _arenas = new List<Arena>();

    public override void InitializeAcademy()
    {
        for (int i = 0; i < ArenaCount; i++)
        {
            var arena = Instantiate(_arenaPrefab, new Vector3((i % 3) * 120, 0, (int)(i / 3) * 100), Quaternion.identity).GetComponent<Arena>();
            arena.Initialize(this);

            _arenas.Add(arena);
        }
    }

    public override void AcademyReset()
    {
        foreach(var arena in _arenas)
        {
            arena.ArenaReset();
        }
    }

    public override void AcademyStep()
    {
        foreach (var arena in _arenas)
        {
            arena.ArenaStep(GetStepCount(), resetParameters);
        }
    }
}

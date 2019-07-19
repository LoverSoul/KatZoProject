using System;

public interface IController
{
    Type ControllerType { get; }
    void Initialize();
    void OnLevelLoad();
    void EnableController();
    void DisableController();
}
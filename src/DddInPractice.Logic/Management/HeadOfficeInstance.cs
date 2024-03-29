﻿namespace DddInPractice.Logic.Management;

// We have only one Head Office for now.
// NOTE this class is in the Repositories layer.
public static class HeadOfficeInstance
{
    private const long HeadOfficeId = 1;
    
    public static HeadOffice Instance { get; private set; }

    public static void Init()
    {
        var repository = new HeadOfficeRepository();
        Instance = repository.GetById(HeadOfficeId);
    }
}
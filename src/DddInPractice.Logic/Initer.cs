﻿namespace DddInPractice.Logic
{
    // Decoupling between UI and utility classes.
    public static class Initer
    {
        public static void Init(string connectionString)
        {
            SessionFactory.Init(connectionString);
        }
    }
}

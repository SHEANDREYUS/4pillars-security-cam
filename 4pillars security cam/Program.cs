using System;
using System.Collections.Generic;


//  ABSTRACTION

public abstract class SecurityCamera
{
    public string Camera { get; }
    public string Location { get; }

    protected SecurityCamera(string cameraId, string location)
    {
        Camera = cameraId;
        Location = location;
    }


    public abstract void StartStream();
    public abstract string GetStatus();
}


// INHERITANCE 

public class PTZCamera : SecurityCamera
{

    // 3. ENCAPSULATION


    private int _panAngle;  // -180 to 180 degrees
    private int _zoomLevel; // 1x to 10x

    public int PanAngle
    {
        get => _panAngle;
        set => _panAngle = Math.Max(-180, Math.Min(value, 180));
        // Max is the lowest
        //Min is the opposite
    }

    public int ZoomLevel
    {
        get => _zoomLevel;
        set => _zoomLevel = Math.Max(1, Math.Min(value, 10));
    }

    public PTZCamera(string id, string location) : base(id, location)
    {
        PanAngle = 90;   // Centered
        ZoomLevel = 1;  // 1x Zoom
    }


    // POLYMORPHISM

    public override void StartStream()
    {
        Console.WriteLine($"[PTZ CAM] {Camera} ({Location})");
        Console.WriteLine($"{PanAngle}° pan angle at {ZoomLevel}x zoom.");
    }

    public override string GetStatus() =>
        $"PTZ Camera [{Camera}] at '{Location}' - Orientation: {PanAngle}°, Zoom: {ZoomLevel}x.";
}


public class ThermalCamera : SecurityCamera
{
    private double Bodyheat;

    public double HeatThresholdCelsius
    {
        get => Bodyheat;
        set => Bodyheat = Math.Max(-20.0, Math.Min(value, 100.0));
    }

    public ThermalCamera(string id, string location, double threshold) : base(id, location)
    {
        HeatThresholdCelsius = threshold;
    }

    public override void StartStream()
    {
        Console.WriteLine($"[THERMAL CAM] {Camera} ({Location})");
        Console.WriteLine($" Infrared array active. Bodyheat: {Bodyheat}°C.");
    }

    public override string GetStatus() =>
        $"Thermal Camera [{Camera}] at '{Location}' - Heat Trigger: {Bodyheat}°C.";
}

// Main Program Execution
public class Program
{
    public static void Main()
    {
        // Polymorphic collection storing specialized security cameras
        List<SecurityCamera> surveillanceNetwork = new List<SecurityCamera>
        {
            new PTZCamera("CAM-01", "Main Gate Parking"),
            new ThermalCamera("CAM-02", "Front door", 37.5)
        };

        Console.WriteLine("=== SECURITY CAMERA STATUS ===");
        Console.WriteLine();

        // Polymorphic loop executing specialized streams across all cameras
        foreach (var camera in surveillanceNetwork)
        {
            camera.StartStream();
            Console.WriteLine(camera.GetStatus());
            Console.WriteLine();
        }
    }
}
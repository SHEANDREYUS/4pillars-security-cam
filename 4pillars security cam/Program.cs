using System;
using System.Collections.Generic;

// ABSTRACTION
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
    // ENCAPSULATION
    private int _panAngle;  // -180 to 180 degrees
    private int _zoomLevel; // 1x to 10x optical zoom

    public int PanAngle
    {
        get => _panAngle;
        set
        {
            if (value > 180)
            {
                _panAngle = 180; 
            }
            else if (value < -180)
            {
                _panAngle = -180; 
            }
            else
            {
                _panAngle = value; 
            }
        }
    }

    public int ZoomLevel
    {
        get => _zoomLevel;
        set
        {
            if (value > 10)
            {
                _zoomLevel = 10; 
            }
            else if (value < 1)
            {
                _zoomLevel = 1;
            }
            else
            {
                _zoomLevel = value; 
            }
        }
    }
    public PTZCamera(string id, string location) : base(id, location)
    {
        PanAngle = 90;
        ZoomLevel = 1;
    }

    // POLYMORPHISM
    public override void StartStream()
    {
        Console.WriteLine($"--------------------------------------");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"|[PTZ CAM] {Camera} ({Location})|");
        Console.ResetColor();
        Console.WriteLine($"--------------------------------------");
        Console.WriteLine($"{PanAngle}° pan angle at {ZoomLevel}x optical zoom.");
    }

    public override string GetStatus() =>
        $"PTZ Camera [{Camera}] at '{Location}' - Orientation: {PanAngle}°, Zoom: {ZoomLevel}x.";
}

// another INHERITANCE: Thermal Camera
public class ThermalCamera : SecurityCamera
{
    private double Bodyheat;
    private int _zoomLevel; // 1x to 5x digital zoom

    public double HeatThresholdCelsius
    {
        get => Bodyheat;
        set
        {
            if (value > 100.0)
            {
                Bodyheat = 100.0; 
            }
            else if (value < -20.0)
            {
                Bodyheat = -20.0; 
            }
            else
            {
                Bodyheat = value; 
            }
        }
    }

    public int ZoomLevel
    {
        get => _zoomLevel;
        set
        {
            if (value > 10)
            {
                _zoomLevel = 10;
            }
            else if (value < 1)
            {
                _zoomLevel = 1;
            }
            else
            {
                _zoomLevel = value;
            }
        }
    }

    public ThermalCamera(string id, string location, double threshold, int initialZoom = 1)
        : base(id, location)
    {
        HeatThresholdCelsius = threshold;
        ZoomLevel = initialZoom;
    }

    public override void StartStream()
    {
        Console.WriteLine($"-----------------------------------");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"|[THERMAL CAM] {Camera} ({Location})|");
        Console.ResetColor();
        Console.WriteLine($"-----------------------------------");
        Console.WriteLine($" Infrared array active. Heat Trigger: {Bodyheat}°C at {ZoomLevel}x digital zoom.");
    }

    public override string GetStatus() =>
        $"Thermal Camera [{Camera}] at '{Location}' - Heat Trigger: {Bodyheat}°C, Zoom: {ZoomLevel}x.";
}


public class Program
{
    public static void Main()
    {
        PTZCamera ptzCam = new PTZCamera("CAM-01", "Main Gate Parking");
        ThermalCamera thermalCam = new ThermalCamera("CAM-02", "Front door", 37.5);

        List<SecurityCamera> surveillanceNetwork = new List<SecurityCamera>
        {
            ptzCam,
            thermalCam
        };

        bool isRunning = true;


        while (isRunning)
        {
            Console.WriteLine("========================================");
           
            Console.WriteLine("       CAMERA SYSTEM MENU      ");

            Console.WriteLine("========================================");
            Console.WriteLine("1. Configure PTZ Camera (Main Gate)");
            Console.WriteLine("\n2. Configure Thermal Camera (Front Door)");
            Console.WriteLine("\n3. View All Live Streams & Status");
            Console.WriteLine("\n4. Exit ");
            Console.Write("\nEnter choice (1-4): ");

            string choice = Console.ReadLine();
            Console.WriteLine();
            Console.Clear();
            switch (choice)
            {

                case "1":
                    Console.Clear();
                    Console.WriteLine("--- Configuring PTZ Camera ---");
                    Console.Write("Enter Pan Angle (-180 to 180): ");
                    ptzCam.PanAngle = int.Parse(Console.ReadLine());
                    
                    Console.Write("Enter Zoom Level (1 to 10): ");
                    ptzCam.ZoomLevel = int.Parse(Console.ReadLine());
                    Console.WriteLine("-> PTZ Camera settings updated!\n");
                    break;

                case "2":
                    Console.Clear();
                    Console.WriteLine("--- Configuring Thermal Camera ---");
                    Console.Write("Enter Thermal Threshold in °C (-20 to 100): ");
                    thermalCam.HeatThresholdCelsius = double.Parse(Console.ReadLine());

                    Console.Write("Enter Digital Zoom Level (1 to 5): ");
                    thermalCam.ZoomLevel = int.Parse(Console.ReadLine());
                    Console.WriteLine("-> Thermal Camera settings updated!\n");
                    break;

                case "3":
                    Console.Clear();
                    Console.WriteLine("=== SECURITY CAMERA STATUS ===\n");
                    foreach (var camera in surveillanceNetwork)
                    {
                        camera.StartStream();
                        Console.WriteLine($"\n{camera.GetStatus()}");
                        Console.WriteLine();
                    }
                    break;

                case "4":
                    Console.Clear();
                    Console.WriteLine("Exiting Camera Control System. ");
                    isRunning = false;
                    break;

                default:
                    Console.WriteLine("Invalid choice! Please select 1, 2, 3, or 4.\n");
                    break;
            }
        }
    }
}

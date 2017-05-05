namespace PhotographyWorkshop.ImportJson
{
    using Data;
    using System;
    using Utilites;
    using System.IO;
    using AutoMapper;
    using System.Linq;
    using Newtonsoft.Json;
    using System.Reflection;
    using System.Collections.Generic;
    using PhotographyWorkshops.Models;
    using System.Text.RegularExpressions;
    using PhotographyWorkshops.Models.DTOs;

    public class Json
    {
        private static IEnumerable<T> ParseJson<T>(string filePath)
        {
            string json = File.ReadAllText(filePath);
            IEnumerable<T> dtos = JsonConvert.DeserializeObject<IEnumerable<T>>(json);

            return dtos;
        }

        public static void ImportLenses()
        {
            IEnumerable<LensDto> lensesDto = ParseJson<LensDto>(Constants.LensPath);
            List<Lens> lenses = new List<Lens>();

            using (PhotographyWorkshopContext context = new PhotographyWorkshopContext())
            {
                foreach (LensDto lensDto in lensesDto)
                {
                    Lens lensEntity = new Lens()
                    {
                        Make = lensDto.Make,
                        FocalLength = int.Parse(lensDto.FocalLength),
                        MaxAperture = float.Parse(lensDto.MaxAperture),
                        CompatibleWith = lensDto.CompatibleWith
                    };

                    lenses.Add(lensEntity);
                    Console.WriteLine($"Successfully imported {lensEntity.Make} {lensEntity.FocalLength}mm f{lensEntity.MaxAperture}");
                }

                context.Lenses.AddRange(lenses);
                context.SaveChanges();
            }
        }

        public static void ImportCameras()
        {
            //Mapper.Initialize(cfg =>
            //{
            //    cfg.CreateMap<CameraDto, CameraDslr>();
            //    cfg.CreateMap<CameraDto, CameraMirrorless>();
            //});

            IEnumerable<CameraDto> CamerasDto = ParseJson<CameraDto>(Constants.CamerasPath);

            List<Camera> cameras = new List<Camera>();

            using (PhotographyWorkshopContext context = new PhotographyWorkshopContext())
            {
                foreach (CameraDto cameraDto in CamerasDto)
                {
                    if (cameraDto.Type == null || cameraDto.Make == null || cameraDto.Model == null ||
                        cameraDto.MinISO == null)
                    {
                        Console.WriteLine(Messages.InvalidDate);
                        continue;
                    }
                    else
                    {
                        Camera cameraEntity = GetCamera(cameraDto);
                        Console.WriteLine($"Successfully imported {cameraEntity.GetType().Name} {cameraEntity.Make} {cameraEntity.Model}");

                        cameras.Add(cameraEntity);
                    }
                }

                context.Cameras.AddRange(cameras);
                context.SaveChanges();
            }
        }

        public static void ImportPhotographers()
        {
            IEnumerable<PhotographerDto> photographerDtos = ParseJson<PhotographerDto>(Constants.PhotographersPath);

            List<Photographer> photographers = new List<Photographer>();

            using (PhotographyWorkshopContext context = new PhotographyWorkshopContext())
            {
                foreach (PhotographerDto photographerDto in photographerDtos)
                {
                    if (photographerDto.FirstName == null || photographerDto.LastName == null)
                    {
                        Console.WriteLine(Messages.InvalidDate);
                        continue;
                    }

                    string phone = String.Empty;
                    if (photographerDto.Phone != null)
                    {
                        Regex regex = new Regex(@"\+\d{1,3}\/\d{8,10}");
                        if (regex.IsMatch(photographerDto.Phone))
                        {
                            phone = photographerDto.Phone;
                        }
                    }

                    Photographer photographerEntity = new Photographer()
                    {
                        FirstName = photographerDto.FirstName,
                        LastName = photographerDto.LastName,
                        Phone = phone
                    };

                    bool alreadyPassedPrimaryCamera = false;
                    photographerEntity.PrimaryCamera = GetRandomCamera(context, alreadyPassedPrimaryCamera);

                    alreadyPassedPrimaryCamera = true;
                    photographerEntity.SecondaryCamera = GetRandomCamera(context, alreadyPassedPrimaryCamera);

                    foreach (var lensDtoId in photographerDto.Lenses)
                    {
                        Lens lensEntity = context.Lenses.FirstOrDefault(lens => lens.Id == lensDtoId);
                        if (lensEntity != null)
                        {
                            if (CheckLensCompatibility(photographerEntity, lensEntity))
                            {
                                photographerEntity.Lenses.Add(lensEntity);
                            }
                        }
                    }

                    Console.WriteLine($"Successfully imported {photographerEntity.FullName} | Lenses: {photographerEntity.Lenses.Count}");
                    photographers.Add(photographerEntity);
                }

                context.Photographers.AddRange(photographers);
                context.SaveChanges();
            }
        }

        private static Camera GetCamera(CameraDto cameraDto)
        {
            // Determine the camera type with reflection:

            Type cameraType = Assembly
                .GetAssembly(typeof(Camera))
                .GetTypes()
                .FirstOrDefault(type => type.Name.ToLower() == ("Camera" + cameraDto.Type).ToLower());

            // All the needed code to create 'CameraDslr' or 'CameraMirrorless' if we use AutoMapper:

            //Object cameraObject = Mapper.Map(cameraDto, cameraDto.GetType(), cameraType);
            //return cameraObject as Camera;

            Camera cameraEntity = new Camera();

            if (cameraType?.Name == "CameraDslr")
            {
                cameraEntity = new CameraDslr()
                {
                    Make = cameraDto.Make,
                    Model = cameraDto.Model,
                    IsFullFrameOrNot = cameraDto.IsFullFrame,
                    MinIso = cameraDto.MinISO ?? 0,
                    MaxIso = cameraDto.MaxISO,
                    MaxShutterSpeed = cameraDto.MaxShutterSpeed
                };
            }
            else if (cameraType?.Name == "CameraMirrorless")
            {
                cameraEntity = new CameraMirrorless()
                {
                    Make = cameraDto.Make,
                    Model = cameraDto.Model,
                    IsFullFrameOrNot = cameraDto.IsFullFrame,
                    MinIso = cameraDto.MinISO ?? 0,
                    MaxIso = cameraDto.MaxISO,
                    MaxVideoResolution = cameraDto.MaxVideoResolution,
                    MaxFrameRate = cameraDto.MaxFrameRate
                };
            }

            return cameraEntity;
        }

        private static Camera GetRandomCamera(PhotographyWorkshopContext context, bool alreadyPassedPrimaryCamera)
        {
            Random rnd = new Random();
            int randomId = rnd.Next(1, context.Cameras.Count() + 1);

            if (alreadyPassedPrimaryCamera)
            {
                if (randomId > 10)
                {
                    randomId -= 10;
                }
                else
                {
                    randomId += 10;
                }
            }

            Camera camera = context.Cameras.Find(randomId);
            return camera;
        }

        private static bool CheckLensCompatibility(Photographer photographer, Lens lens)
        {
            if (lens.CompatibleWith == photographer.PrimaryCamera.Make || lens.CompatibleWith == photographer.SecondaryCamera.Make)
            {
                return true;
            }

            return false;
        }
    }
}

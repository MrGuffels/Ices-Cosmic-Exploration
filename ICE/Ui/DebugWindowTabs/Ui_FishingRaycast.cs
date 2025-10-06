using FFXIVClientStructs.FFXIV.Client.System.Framework;
using FFXIVClientStructs.FFXIV.Common.Component.BGCollision;
using System.Collections.Generic;

namespace ICE.Ui.DebugWindowTabs
{
    public unsafe class FishingDebug : IDisposable
    {
        private delegate* unmanaged<BGCollisionModule*, RaycastHit*, Vector3*, Vector3*, float, int, byte> _raycastSimple;
        public bool ShowFishRay = true;

        public const uint Success = 0xFF00FF00;
        public const uint Failure = 0xFF00FFFF;

        public FishingDebug()
        {
            try
            {
                _raycastSimple = (delegate* unmanaged<BGCollisionModule*, RaycastHit*, Vector3*, Vector3*, float, int, byte>)Svc.SigScanner.ScanText("E8 ?? ?? ?? ?? 84 C0 75 58 FF C3");
                Svc.Log.Info("Fishing raycast initialized successfully");
            }
            catch (Exception ex)
            {
                Svc.Log.Error($"Failed to initialize fishing raycast: {ex}");
            }
        }

        private static Vector3 TransformVecByMatrix(Vector3 a2, Matrix4x4 a3)
        {
            var v6 = (a2.X * a3.M13) + (a2.Y * a3.M23) + (a2.Z * a3.M33) + a3.M43;
            var v7 = (a2.X * a3.M12) + (a2.Y * a3.M22) + (a2.Z * a3.M32) + a3.M42;
            var v8 = (a2.Y * a3.M21) + (a2.X * a3.M11) + (a2.Z * a3.M31) + a3.M41;
            return new Vector3()
            {
                X = v8,
                Y = v7,
                Z = v6
            };
        }

        /// <summary>
        /// Check if the current facing direction is fishable
        /// </summary>
        /// <param name="rotation">Player rotation to check (defaults to current player rotation)</param>
        /// <returns>True if fishable, false otherwise</returns>
        public bool IsFishable(float? rotation = null)
        {
            if (Svc.ClientState.LocalPlayer is not { } player)
                return false;

            if (_raycastSimple == null)
                return false;

            var position = player.Position;
            var rot = rotation ?? player.Rotation;

            return CheckFishableAtRotation(position, rot, out _);
        }

        /// <summary>
        /// Find a fishable location within 360 degrees around the player
        /// </summary>
        /// <param name="fishablePosition">Output: The fishable position if found</param>
        /// <param name="searchSteps">Number of angles to check (higher = more accurate but slower)</param>
        /// <returns>True if a fishable location was found</returns>
        public bool FindFishableLocation(out Vector3? fishablePosition, int searchSteps = 36)
        {
            fishablePosition = null;

            if (Svc.ClientState.LocalPlayer is not { } player)
                return false;

            if (_raycastSimple == null)
                return false;

            var position = player.Position;
            var angleStep = (2 * MathF.PI) / searchSteps;

            // Check all angles around the player
            for (int i = 0; i < searchSteps; i++)
            {
                var angle = i * angleStep;

                if (CheckFishableAtRotation(position, angle, out var hitPoint))
                {
                    fishablePosition = hitPoint;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get all fishable locations within 360 degrees
        /// </summary>
        /// <param name="searchSteps">Number of angles to check</param>
        /// <returns>List of fishable positions with their corresponding rotations</returns>
        public List<(Vector3 Position, float Rotation)> GetAllFishableLocations(int searchSteps = 36)
        {
            var fishableLocations = new List<(Vector3, float)>();

            if (Svc.ClientState.LocalPlayer is not { } player)
                return fishableLocations;

            if (_raycastSimple == null)
                return fishableLocations;

            var position = player.Position;
            var angleStep = (2 * MathF.PI) / searchSteps;

            for (int i = 0; i < searchSteps; i++)
            {
                var angle = i * angleStep;

                if (CheckFishableAtRotation(position, angle, out var hitPoint))
                {
                    fishableLocations.Add((hitPoint.Value, angle));
                }
            }

            return fishableLocations;
        }

        private bool CheckFishableAtRotation(Vector3 position, float rotation, out Vector3? hitPoint)
        {
            hitPoint = null;

            var v8 = MathF.Cos(rotation);
            var v9 = MathF.Sin(rotation);

            var playerRotationMatrix = new Matrix4x4()
            {
                M11 = v8,
                M13 = -v9,
                M31 = v9,
                M33 = v8,
                M22 = 1.0f
            };

            var v43 = new Vector3()
            {
                Z = 2.0f
            };

            var rodPoint = TransformVecByMatrix(v43, playerRotationMatrix);
            rodPoint += position;
            rodPoint.Y += 2;

            var playerRayOrigin = new Vector3()
            {
                X = position.X,
                Y = position.Y + 0.87f,
                Z = position.Z
            };

            var rodDirection = rodPoint - playerRayOrigin;
            rodDirection /= rodDirection.Length();

            // Check if line of sight is blocked
            if (BGCollisionModule.RaycastMaterialFilter(playerRayOrigin, rodDirection, out var hitInfo, 2))
            {
                return false;
            }

            var fishRay = TransformVecByMatrix(new Vector3(0, -80, 40), playerRotationMatrix);
            var fishRayLen = fishRay.Length();
            var fishRayNormalized = fishRay / fishRayLen;

            RaycastHit castHitInfo;

            if (_raycastSimple(Framework.Instance()->BGCollisionModule, &castHitInfo, &rodPoint, &fishRayNormalized, fishRayLen, 1) == 1)
            {
                // Check if material is fishable
                if ((castHitInfo.Material & 0x8000) != 0)
                {
                    hitPoint = castHitInfo.Point;
                    return true;
                }

                // Special case for material 0x2000
                if (castHitInfo.Material == 0x2000)
                {
                    var extraHitTest = castHitInfo.Point + (fishRayNormalized * 0.01f);
                    RaycastHit castHitInfo2;

                    if (_raycastSimple(Framework.Instance()->BGCollisionModule, &castHitInfo2, &extraHitTest, &fishRayNormalized, fishRayLen, 1) == 1)
                    {
                        if ((castHitInfo2.Material & 0x8000) != 0)
                        {
                            hitPoint = castHitInfo2.Point;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public void Draw()
        {
            if (Svc.ClientState.LocalPlayer is not { } player)
                return;

            if (!ShowFishRay)
                return;

            if (_raycastSimple == null)
            {
                ImGui.TextColored(new Vector4(1, 0, 0, 1), "Raycast not initialized!");
                return;
            }

            var position = player.Position;
            var rotation = player.Rotation;

            var v8 = MathF.Cos(rotation);
            var v9 = MathF.Sin(rotation);

            var playerRotationMatrix = new Matrix4x4()
            {
                M11 = v8,
                M13 = -v9,
                M31 = v9,
                M33 = v8,
                M22 = 1.0f
            };

            var v43 = new Vector3()
            {
                Z = 2.0f
            };

            var rodPoint = TransformVecByMatrix(v43, playerRotationMatrix);
            rodPoint += position;
            rodPoint.Y += 2;

            static void drawVector(Vector3 a, Vector3 b, uint color)
            {
                Svc.GameGui.WorldToScreen(a, out var posScreen);
                Svc.GameGui.WorldToScreen(b, out var normalScreen);
                ImGui.GetBackgroundDrawList().AddLine(posScreen, normalScreen, color, 3);
            }

            static void drawPoint(Vector3 a, uint color)
            {
                if (Svc.GameGui.WorldToScreen(a, out var pos))
                    ImGui.GetBackgroundDrawList().AddCircle(pos, 10, color, 2f);
            }

            var playerRayOrigin = new Vector3()
            {
                X = position.X,
                Y = position.Y + 0.87f,
                Z = position.Z
            };

            void drawCast(Vector3 pointA, Vector3? pointB, uint color)
            {
                if (pointB is { } p)
                {
                    drawVector(playerRayOrigin, pointA, color);
                    drawVector(pointA, p, color);
                    drawPoint(p, color);
                }
                else
                {
                    drawVector(playerRayOrigin, pointA, color);
                    drawPoint(pointA, color);
                }
            }

            var rodDirection = rodPoint - playerRayOrigin;
            rodDirection /= rodDirection.Length();

            if (BGCollisionModule.RaycastMaterialFilter(playerRayOrigin, rodDirection, out var hitInfo, 2))
            {
                drawCast(hitInfo.Point, null, Failure);
                return;
            }

            var fishRay = TransformVecByMatrix(new Vector3(0, -80, 40), playerRotationMatrix);
            var fishRayLen = fishRay.Length();
            var fishRayNormalized = fishRay / fishRayLen;

            RaycastHit castHitInfo;

            if (_raycastSimple(Framework.Instance()->BGCollisionModule, &castHitInfo, &rodPoint, &fishRayNormalized, fishRayLen, 1) == 1)
            {
                if ((castHitInfo.Material & 0x8000) != 0)
                {
                    drawCast(rodPoint, castHitInfo.Point, Success);
                    return;
                }

                if (castHitInfo.Material == 0x2000)
                {
                    var extraHitTest = castHitInfo.Point + (fishRayNormalized * 0.01f);
                    RaycastHit castHitInfo2;

                    if (_raycastSimple(Framework.Instance()->BGCollisionModule, &castHitInfo2, &extraHitTest, &fishRayNormalized, fishRayLen, 1) == 1)
                    {
                        drawCast(rodPoint, castHitInfo2.Point, (castHitInfo2.Material & 0x8000) == 0 ? Failure : Success);
                        return;
                    }
                    return;
                }

                drawCast(rodPoint, castHitInfo.Point, Failure);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
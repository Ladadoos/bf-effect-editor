// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models.Elements;
using BattleForgeEffectEditor.Models.Utility;
using System;
using System.Collections.Generic;
using System.IO;

namespace BattleForgeEffectEditor.Models.DataAccess
{
    public class SpecialEffectExportService
    {
        public void ExportEffect(SpecialEffect effect, string fullPath)
        {
            using(FileStream fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            using(BinaryWriter writer = new BinaryWriter(fileStream))
            {
                writer.Write(SpecialEffectImportService.FXBHeader);
                writer.Write(1); //Number of effects

                //Leave 8 bytes to write the node information and hierarchy at the end
                writer.Seek(4 + 4 + 4 + 4, SeekOrigin.Begin);

                writer.Write(2); //Node count
                writer.Write(1); //Unknown hardcoded one
                writer.Write(SpecialEffect.Header);
                writer.Write(2); //Unknown hardcoded two
                writer.WriteBfString(effect.Name);
                writer.Write(effect.Length);
                writer.WriteBfString(effect.SetupFileName);
                writer.Write(effect.SetupSourceId);
                writer.Write(effect.SetupTargetId);
                writer.Write(effect.PlayLength);
                writer.Write(0); //Unknown hardcoded zero
                writer.Write(0); //Unknown hardcoded zero
                writer.Write(Track.StartTracks);
                writer.Write(Track.StartTracks);
                WriteStaticTracks(writer, effect.StaticTracks);
                WriteTracks(writer, effect.Tracks);

                //Element tree
                writer.Write(Element.StartElementChildrenHeader);
                foreach (Element element in effect.Children)
                    WriteElement(writer, element);
                writer.Write(Element.EndElementChildrenHeader);
                writer.Write(Element.EndElementChildrenHeader);

                uint nodeInformationOffset = (uint)writer.BaseStream.Position;
                //Root node information
                writer.Write(0L); //Unknown hardcoded 8 zeroes
                writer.Write(0L); //Unknown hardcoded 8 zeroes
                writer.Write(-1); //Unknown hardcoded negative one
                writer.Write(1u); //Unknown hardcoded one
                writer.Write(1u); //Unknown hardcoded one
                writer.Write(0u); //Unknown hardcoded zero

                //Fx node master information
                writer.Write(0xAB125265);
                writer.Write(1u); //Identifier
                writer.Write(20u); //Byte offset
                writer.Write(nodeInformationOffset - 20); //Byte size
                writer.Write(0L); //Unknown hardcoded 8 zeroes
                writer.Write(0L); //Unknown hardcoded 8 zeroes

                uint nodeHiearchyOffset = (uint)writer.BaseStream.Position;
                writer.Write(0); //Unknown hardcoded zero
                writer.Write(0); //Unknown hardcoded zero
                writer.WriteBfString(new BfString("root node"));

                writer.Write(1); //Unknown hardcoded one
                writer.WriteBfString(new BfString("FxMaster"));
                writer.Write(0); //Unknown hardcoded zero

                writer.Seek(4 + 4, SeekOrigin.Begin);
                writer.Write(nodeInformationOffset);
                writer.Write(nodeHiearchyOffset);
            }
        }

        private void WriteElement(BinaryWriter writer, Element element)
        {
            if (!element.IsEnabled)
                return;

            WriteNodeLink(writer, element.NodeLink);
            writer.Write(Element.StartElementHeader);
            writer.Write(1); //Unknown hardcoded one
            writer.WriteBfString(element.Name);
            WriteElementSettings(writer, element);
            writer.Write(Element.EndElementHeader);

            // AnimatedMesh and AnimatedMeshMaterial have an extra header
            writer.Write(Track.StartTracks);
            if (!(element is AnimatedMesh))
                writer.Write(Track.StartTracks);

            // AnimatedMeshMaterial would write its Materials here
            // While MeshMaterial would do it before writing the element

            WriteStaticTracks(writer, element.StaticTracks);
            WriteTracks(writer, element.Tracks);

            writer.Write(Element.StartElementChildrenHeader);
            foreach (Element child in element.Children)
                WriteElement(writer, child);
            writer.Write(Element.EndElementChildrenHeader);

            // Effect has an extra end of children header
            if (element is Effect)
                writer.Write(Element.EndElementChildrenHeader);
        }

        private void WriteElementSettings(BinaryWriter writer, Element element)
        {
            if (element is AnimatedMesh animatedMesh)
            {
                writer.Write(AnimatedMesh.Header);
                writer.Write(1);
                writer.WriteBfString(animatedMesh.MeshFilePath);
                writer.WriteBfString(animatedMesh.AnimationFilePath);
            } else if (element is Billboard billboard)
            {
                writer.Write(Billboard.Header);
                writer.Write(2);
                writer.WriteBfString(billboard.TextureOneFilePath);
                writer.WriteBfString(billboard.TextureTwoFilePath);
            } else if (element is CameraShake)
            {
                writer.Write(CameraShake.Header);
                writer.Write(1);
            } else if (element is Decal decal)
            {
                writer.Write(Decal.Header);
                writer.Write(1);
                writer.WriteBfString(decal.TextureFilePath);
            } else if (element is Effect effect)
            {
                writer.Write(Effect.Header);
                writer.Write(1);
                writer.WriteBfString(effect.EffectFilePath);
                writer.Write(effect.Embedded);
                writer.Write(effect.Length);
            } else if (element is Emitter emitter)
            {
                writer.Write(Emitter.Header);
                writer.Write(1);
                writer.WriteBfString(emitter.TextureFilePath);
                writer.Write(emitter.ParticleCount);
            } else if (element is Force)
            {
                writer.Write(Force.Header);
                writer.Write(1);
            } else if (element is ForcePoint)
            {
                writer.Write(ForcePoint.Header);
                writer.Write(1);
            } else if (element is Light light)
            {
                writer.Write(Light.Header);
                writer.Write(light.Range);
                writer.Write(light.Radiance);
            } else if (element is Mesh mesh)
            {
                writer.Write(Mesh.Header);
                writer.Write(1);
                writer.WriteBfString(mesh.MeshFilePath);
            } else if (element is Physic physic)
            {
                writer.Write(Physic.Header);
                writer.Write(1);
                writer.WriteBfString(physic.MeshFilePath);
            } else if (element is PhysicGroup)
            {
                writer.Write(PhysicGroup.Header);
                writer.Write(1);
            } else if (element is SfpEmitter)
            {
                writer.Write(SfpEmitter.Header);
                writer.Write(1);
            } else if (element is SfpForceField)
            {
                writer.Write(SfpForceField.Header);
                writer.Write(1);
            } else if (element is SfpSystem sfpSystem)
            {
                writer.Write(SfpSystem.Header);
                writer.Write(1);
                writer.WriteBfString(sfpSystem.TextureFilePath);
            } else if (element is Sound sound)
            {
                writer.Write(Sound.Header);
                writer.Write(1);
                writer.WriteBfString(sound.SoundFilePath);
            } else if (element is StaticDecal staticDecal)
            {
                writer.Write(StaticDecal.Header);
                bool hasNormal = staticDecal.NormalTextureFilePath.Text.Length != 0;
                writer.Write(hasNormal ? 2 : 1);
                writer.WriteBfString(staticDecal.ColorTextureFilePath);
                if (hasNormal)
                    writer.WriteBfString(staticDecal.NormalTextureFilePath);
            } else if (element is Trail trail)
            {
                writer.Write(Trail.Header);
                writer.Write(1);
                writer.WriteBfString(trail.TextureFilePath);
            } else if (element is WaterDecal waterDecal)
            {
                writer.Write(WaterDecal.Header);
                writer.Write(1);
                writer.WriteBfString(waterDecal.TextureFilePath);
            } else
            {
                throw new NotImplementedException(element.GetType().ToString());
            }
        }

        private void WriteStaticTracks(BinaryWriter writer, List<IStaticTrack> tracks)
        {
            foreach (IStaticTrack staticTrack in tracks)
            {
                writer.Write(StaticTrack.Header);
                writer.Write(1); //Unknown hardcoded one
                writer.WriteEnumValue(staticTrack.TrackType);

                if (staticTrack is FloatStaticTrack floatTrack)
                {
                    writer.Write(FloatStaticTrack.FloatHeader);
                    writer.Write(floatTrack.Data);
                } else if (staticTrack is Vector3StaticTrack vectorTrack)
                {
                    writer.Write(Vector3StaticTrack.Vector3Header);
                    writer.Write(vectorTrack.Data);
                } else if (staticTrack is StringStaticTrack stringTrack)
                {
                    writer.Write(StringStaticTrack.StringHeader);
                    writer.WriteBfString(stringTrack.Data);
                } else if (staticTrack is Vector3OtherStaticTrack vectorOtherTrack)
                {
                    writer.Write(Vector3OtherStaticTrack.Vector3OtherHeader);
                    writer.Write(vectorOtherTrack.Data);
                } else
                    throw new NotImplementedException();
            }
        }

        private void WriteTracks(BinaryWriter writer, List<Track> tracks)
        {
            foreach (Track track in tracks)
            {
                if (!track.IsEnabled)
                    continue;

                if (track.EntryKeyFrames.Count == 0)
                    throw new UnexpectedDataException("Track " + track.TrackType + " must have at least one frame!");

                writer.Write(Track.Header);
                writer.Write(4); //Unknown hardcoded four
                writer.WriteEnumValue(track.TrackType);
                writer.Write(track.Length);
                writer.WriteEnumValue(track.Dim);
                writer.WriteEnumValue(track.Mode);
                writer.WriteEnumValue(track.InterpolationType);
                writer.WriteEnumValue(track.EvaluationType);

                if (track.EntryKeyFrames[0] is FloatKeyFrame)
                {
                    foreach (ITrackKeyFrame keyframe in track.EntryKeyFrames)
                    {
                        writer.Write(FloatKeyFrame.EntryHeader);

                        FloatKeyFrame floatKeyFrame = (FloatKeyFrame)keyframe;
                        writer.Write(floatKeyFrame.Frame);
                        writer.Write(floatKeyFrame.Data);
                    }

                    if (track.ControlPointKeyFrames.Count != 0)
                    {
                        writer.Write(FloatKeyFrame.StartControlPointHeader);

                        foreach (ITrackKeyFrame keyframe in track.ControlPointKeyFrames)
                        {
                            writer.Write(FloatKeyFrame.ControlPointHeader);

                            FloatKeyFrame floatKeyFrame = (FloatKeyFrame)keyframe;
                            writer.Write(floatKeyFrame.Frame);
                            writer.Write(floatKeyFrame.Data);
                        }
                    }
                    writer.Write(TrackKeyFrame.EndControlPointHeader);
                } else if (track.EntryKeyFrames[0] is Vector3KeyFrame)
                {
                    foreach (ITrackKeyFrame keyframe in track.EntryKeyFrames)
                    {
                        writer.Write(Vector3KeyFrame.EntryHeader);

                        Vector3KeyFrame vectorKeyFrame = (Vector3KeyFrame)keyframe;
                        writer.Write(vectorKeyFrame.Frame);
                        writer.Write(vectorKeyFrame.Data);
                    }

                    if (track.ControlPointKeyFrames.Count != 0)
                    {
                        writer.Write(Vector3KeyFrame.StartControlPointHeader);

                        foreach (ITrackKeyFrame keyframe in track.ControlPointKeyFrames)
                        {
                            writer.Write(Vector3KeyFrame.ControlPointHeader);

                            Vector3KeyFrame vectorKeyFrame = (Vector3KeyFrame)keyframe;
                            writer.Write(vectorKeyFrame.Frame);
                            writer.Write(vectorKeyFrame.Data);
                        }
                    }
                    writer.Write(TrackKeyFrame.EndControlPointHeader);
                } else
                    throw new NotImplementedException();
            }
        }

        private void WriteNodeLink(BinaryWriter writer, NodeLink nodeLink)
        {
            writer.Write(NodeLink.Header);
            writer.Write(nodeLink.UnknownBitField);
            writer.WriteBfString(nodeLink.Parent);
            writer.WriteBfString(nodeLink.Slot);
            writer.WriteBfString(nodeLink.DestinationSlot);
            writer.Write(nodeLink.World);
            writer.Write(nodeLink.Node);
            writer.Write(nodeLink.Floor);
            writer.Write(nodeLink.Aim);
            writer.Write(nodeLink.Span);
            if (nodeLink.UnknownBitField > 2)
                writer.Write(nodeLink.Locator);
        }
    }
}

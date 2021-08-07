// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Models.Elements;
using BattleForgeEffectEditor.Models.Enums;
using BattleForgeEffectEditor.Models.Utility;
using System;
using System.Collections.Generic;
using System.IO;

namespace BattleForgeEffectEditor.Models.DataAccess
{
    public class SpecialEffectImportService
    {
        public const uint FXBHeader = 0xC57CF11E;

        public SpecialEffect LoadEffect(string fullPath)
        {
            using (FileStream fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            using (BinaryReader reader = new BinaryReader(fileStream))
            {
                uint fileSignature = reader.ReadUInt32();
                uint numberOfEffects = reader.ReadUInt32();
                if (numberOfEffects != 1)
                    throw new UnexpectedDataException("More than one effect counted " + numberOfEffects);
                uint nodeInformationOffset = reader.ReadUInt32();
                uint nodeHierarchyOffset = reader.ReadUInt32();
                uint nodeCount = reader.ReadUInt32();
                if (nodeCount != 2)
                    throw new UnexpectedDataException("More than two nodes counted " + nodeCount);

                reader.BaseStream.Seek(nodeInformationOffset, SeekOrigin.Begin);
                ReadRootNodeInfo(reader);
                uint byteOffset = ReadFxMasterNodeInfo(reader);

                reader.BaseStream.Seek(nodeHierarchyOffset, SeekOrigin.Begin);
                uint rootnodeIdentifier = reader.ReadUInt32();
                uint zero = reader.ReadUInt32();
                BfString rootNodeName = reader.ReadBfString();

                uint fxNodeMasterId = reader.ReadUInt32();
                BfString fxNodeMasterName = reader.ReadBfString();
                uint fxNodeMasterZero = reader.ReadUInt32();

                reader.BaseStream.Seek(byteOffset, SeekOrigin.Begin);
                uint effectOne = reader.ReadUInt32();
                if (effectOne != 1)
                    throw new UnexpectedDataException("Expected one in effect header, found " + effectOne);
                uint header = reader.ReadUInt32();
                if (header != SpecialEffect.Header)
                    throw new UnexpectedDataException("Expected " + SpecialEffect.Header.ToString("X") + " for effect header, found " + header.ToString("X"));
                uint two = reader.ReadUInt32();
                if (two != 2)
                    throw new UnexpectedDataException("Expected two in effect header, found " + two);
                BfString name = reader.ReadBfString();
                float length = reader.ReadSingle();
                BfString setupFileName = reader.ReadBfString();
                int setupSourceId = reader.ReadInt32();
                int setupTargetId = reader.ReadInt32();
                float playLength = reader.ReadSingle();
                int unknownOne = reader.ReadInt32();
                if (unknownOne != 0)
                    throw new UnexpectedDataException("Expected zero in unknown one, found " + unknownOne);
                int unknownTwo = reader.ReadInt32();
                if (unknownTwo != 0)
                    throw new UnexpectedDataException("Expected zero in unknown two, found " + unknownOne);

                uint header_F8575767 = reader.ReadUInt32();
                if (header_F8575767 != Track.StartTracks)
                    throw new UnexpectedDataException("Expected first track header " + Track.StartTracks.ToString("X") +
                        " in effect, found " + header_F8575767.ToString("X"));
                uint headerTwo_F8575767 = reader.ReadUInt32();
                if (headerTwo_F8575767 != Track.StartTracks)
                    throw new UnexpectedDataException("Expected second track header " + Track.StartTracks.ToString("X") +
                        " in effect, found " + headerTwo_F8575767.ToString("X"));

                List<IStaticTrack> effectStaticTrack = ReadStaticTracks(reader);
                List<Track> effectTrack = ReadTracks(reader);

                uint startChildrenHeader = reader.ReadUInt32();
                if (startChildrenHeader != Element.StartElementChildrenHeader)
                    throw new UnexpectedDataException("Expected start special effect header " + Element.StartElementChildrenHeader.ToString("X") +
                        " in effect, found " + startChildrenHeader.ToString("X"));

                SpecialEffect specialEffect = new SpecialEffect(name, length, playLength, setupFileName, setupSourceId, setupTargetId,
                    effectStaticTrack, effectTrack);

                if (reader.Peek<uint>() != Element.EndElementChildrenHeader)
                {
                    int depth = 1;
                    int[] ignores = new int[16];
                    ReadElement(reader, specialEffect, ignores, depth);
                }
                return specialEffect;
            }
        }

        private void ReadRootNodeInfo(BinaryReader reader)
        {
            long eightZeroes = reader.ReadInt64();
            long eightZeroesTwo = reader.ReadInt64();
            int negativeOne = reader.ReadInt32();
            uint one = reader.ReadUInt32();
            uint oneTwo = reader.ReadUInt32();
            uint zero = reader.ReadUInt32();
        }

        private uint ReadFxMasterNodeInfo(BinaryReader reader)
        {
            uint magic = reader.ReadUInt32();
            uint identifier = reader.ReadUInt32();
            uint byteOffset = reader.ReadUInt32();
            uint byteSize = reader.ReadUInt32();
            long eightZeroes = reader.ReadInt64();
            long eightZeroesTwo = reader.ReadInt64();
            return byteOffset;
        }

        private void ReadElement(BinaryReader reader, IElement parent, int[] ignores, int depth)
        {
            NodeLink nodeLink = ReadNodeLink(reader);
            uint startElementHeader = reader.ReadUInt32();
            if (startElementHeader != Element.StartElementHeader)
                throw new UnexpectedDataException("Expected start element header " + Element.StartElementHeader.ToString("X") +
                    " in element, found " + startElementHeader.ToString("X"));
            uint oneElement = reader.ReadUInt32();
            if (oneElement != 1)
                throw new UnexpectedDataException("Expected one in element, found " + oneElement);
            BfString elementName = reader.ReadBfString();
            uint elementTypeHeader = reader.ReadUInt32();

            Element element = null;
            switch (elementTypeHeader)
            {
                case Light.Header:
                {
                    uint range = reader.ReadUInt32();
                    float radiance = reader.ReadSingle();
                    element = new Light(range, radiance);
                    break;
                }
                case StaticDecal.Header:
                {
                    uint oneOrTwo = reader.ReadUInt32();
                    if (oneOrTwo != 1 && oneOrTwo != 2)
                        throw new UnexpectedDataException("Expected one or two in static decal, found " + oneOrTwo);
                    BfString colorTextureFilePath = reader.ReadBfString();
                    BfString normalTextureFilePath = new BfString();
                    if (oneOrTwo == 2)
                        normalTextureFilePath = reader.ReadBfString();
                    element = new StaticDecal(colorTextureFilePath, normalTextureFilePath);
                    break;
                }
                case Sound.Header:
                {
                    uint one = reader.ReadUInt32();
                    if (one != 1)
                        throw new UnexpectedDataException("Expected one in sound, found " + one);
                    BfString soundFilePath = reader.ReadBfString();
                    element = new Sound(soundFilePath);
                    break;
                }
                case Billboard.Header:
                {
                    uint oneOrTwo = reader.ReadUInt32();
                    if (oneOrTwo != 1 && oneOrTwo != 2)
                        throw new UnexpectedDataException("Expected one or two in billboard, found " + oneOrTwo);
                    BfString textureOneFilePath = reader.ReadBfString();
                    BfString textureTwoFilePath = new BfString();
                    if (oneOrTwo == 2)
                        textureTwoFilePath = reader.ReadBfString();
                    element = new Billboard(textureOneFilePath, textureTwoFilePath);
                    break;
                }
                case Emitter.Header:
                {
                    uint one = reader.ReadUInt32();
                    if (one != 1)
                        throw new UnexpectedDataException("Expected one in emitter, found " + one);
                    BfString textureFilePath = reader.ReadBfString();
                    uint particleCount = reader.ReadUInt32();
                    element = new Emitter(textureFilePath, particleCount);
                    break;
                }
                case CameraShake.Header:
                {
                    uint one = reader.ReadUInt32();
                    if (one != 1)
                        throw new UnexpectedDataException("Expected one in camera shake, found " + one);
                    element = new CameraShake();
                    break;
                }
                case Mesh.Header:
                {
                    uint one = reader.ReadUInt32();
                    if (one != 1)
                        throw new UnexpectedDataException("Expected one in mesh, found " + one);
                    BfString meshFilePath = reader.ReadBfString();
                    element = new Mesh(meshFilePath);
                    break;
                }
                case Effect.Header:
                {
                    uint one = reader.ReadUInt32();
                    if (one != 1)
                        throw new UnexpectedDataException("Expected one in effect, found " + one);
                    BfString effectFilePath = reader.ReadBfString();
                    uint embedded = reader.ReadUInt32();
                    float effectLength = reader.ReadSingle();
                    element = new Effect(effectFilePath, embedded, effectLength);
                    break;
                }
                case Trail.Header:
                {
                    uint one = reader.ReadUInt32();
                    if (one != 1)
                        throw new UnexpectedDataException("Expected one in trail, found " + one);
                    BfString textureFilePath = reader.ReadBfString();
                    element = new Trail(textureFilePath);
                    break;
                }
                case PhysicGroup.Header:
                {
                    uint one = reader.ReadUInt32();
                    if (one != 1)
                        throw new UnexpectedDataException("Expected one in physic group, found " + one);
                    element = new PhysicGroup();
                    break;
                }
                case Physic.Header:
                {
                    uint one = reader.ReadUInt32();
                    if (one != 1)
                        throw new UnexpectedDataException("Expected one in physic, found " + one);
                    BfString meshFilePath = reader.ReadBfString();
                    element = new Physic(meshFilePath);
                    break;
                }
                case Decal.Header:
                {
                    uint one = reader.ReadUInt32();
                    if (one != 1)
                        throw new UnexpectedDataException("Expected one in decal, found " + one);
                    BfString textureFilePath = reader.ReadBfString();
                    element = new Decal(textureFilePath);
                    break;
                }
                case Force.Header:
                {
                    uint one = reader.ReadUInt32();
                    if (one != 1)
                        throw new UnexpectedDataException("Expected one in force, found " + one);
                    element = new Force();
                    break;
                }
                case ForcePoint.Header:
                {
                    uint one = reader.ReadUInt32();
                    if (one != 1)
                        throw new UnexpectedDataException("Expected one in force point, found " + one);
                    element = new ForcePoint();
                    break;
                }
                case AnimatedMesh.Header:
                {
                    uint one = reader.ReadUInt32();
                    if (one != 1)
                        throw new UnexpectedDataException("Expected one in animated mesh, found " + one);
                    BfString meshFilePath = reader.ReadBfString();
                    BfString animationFilePath = reader.ReadBfString();
                    element = new AnimatedMesh(meshFilePath, animationFilePath);
                    break;
                }
                case WaterDecal.Header:
                {
                    uint one = reader.ReadUInt32();
                    if (one != 1)
                        throw new UnexpectedDataException("Expected one in water decal, found " + one);
                    BfString textureFilePath = reader.ReadBfString();
                    element = new WaterDecal(textureFilePath);
                    break;
                }
                case SfpSystem.Header:
                {
                    uint one = reader.ReadUInt32();
                    if (one != 1)
                        throw new UnexpectedDataException("Expected one in sfpsystem, found " + one);
                    BfString textureFilePath = reader.ReadBfString();
                    element = new SfpSystem(textureFilePath);
                    break;
                }
                case SfpEmitter.Header:
                {
                    uint one = reader.ReadUInt32();
                    if (one != 1)
                        throw new UnexpectedDataException("Expected one in sfpemitter, found " + one);
                    element = new SfpEmitter();
                    break;
                }
                case SfpForceField.Header:
                {
                    uint one = reader.ReadUInt32();
                    if (one != 1)
                        throw new UnexpectedDataException("Expected one in sfpforcefield, found " + one);
                    element = new SfpForceField();
                    break;
                }
                default:
                    throw new InvalidDataException("Invalid element with header " + elementTypeHeader.ToString("X"));
            }

            uint headerOne = reader.ReadUInt32();
            if (headerOne != Element.EndElementHeader)
                throw new UnexpectedDataException("Expected zero track header " + Element.EndElementHeader.ToString("X") +
                    " in element, found " + headerOne.ToString("X"));

            //AnimatedMesh and AnimatedMeshMaterial should have 1 time, the rest should have 2
            uint trackCounter = 0;
            while (reader.Peek<uint>() == Track.StartTracks)
            {
                uint headerTrack = reader.ReadUInt32();
                trackCounter++;
            }
            if (trackCounter != 2 && !(element is AnimatedMesh))
                throw new UnexpectedDataException("Expected track counter of 2 for " + element.GetType().Name +
                    " but found " + trackCounter);

            List<IStaticTrack> elementStaticTrack = ReadStaticTracks(reader);
            List<Track> elementEffectTrack = ReadTracks(reader);
            element.Set(elementName, nodeLink, elementStaticTrack, elementEffectTrack);
            parent.Children.Add(element);
            element.Parent = parent;

            uint startChildrenHeader = reader.ReadUInt32();
            if (startChildrenHeader != Element.StartElementChildrenHeader)
                throw new UnexpectedDataException("Expectected start of element child header, but found" +
                    startChildrenHeader.ToString("X"));

            // An extra end header is written after an Effect
            if (element is Effect)
                ignores[depth]++;

            IElement nextParent = element;
            depth++;
            while (reader.Peek<uint>() == Element.EndElementChildrenHeader)
            {
                uint endChildHeader = reader.ReadUInt32();
                if (ignores[depth] > 0)
                    ignores[depth]--;
                else
                {
                    if (nextParent != null)
                        nextParent = nextParent.Parent;
                    else
                    {
                        if (depth != 0)
                            throw new Exception();
                    }
                    depth--;
                }
            }
            if (depth == -1)
                return;

            ReadElement(reader, nextParent, ignores, depth);
        }

        private List<Track> ReadTracks(BinaryReader reader)
        {
            List<Track> tracks = new List<Track>();

            while (reader.Peek<uint>() == Track.Header)
            {
                uint header = reader.ReadUInt32();
                uint four = reader.ReadUInt32();
                if (four != 4)
                    throw new UnexpectedDataException("Expected four in track, found " + four);
                TrackType trackType = reader.ReadEnumValue<TrackType>();
                float length = reader.ReadSingle();
                TrackDim trackDim = reader.ReadEnumValue<TrackDim>();
                TrackMode trackMode = reader.ReadEnumValue<TrackMode>();
                TrackInterpolationType interpolationType = reader.ReadEnumValue<TrackInterpolationType>();
                TrackEvaluationType evaluationType = reader.ReadEnumValue<TrackEvaluationType>();

                List<ITrackKeyFrame> entries = new List<ITrackKeyFrame>();
                List<ITrackKeyFrame> controlPoints = new List<ITrackKeyFrame>();

                uint dataHeader = reader.Peek<uint>();
                if (dataHeader == FloatKeyFrame.EntryHeader)
                {
                    while (reader.Peek<uint>() == FloatKeyFrame.EntryHeader)
                    {
                        uint floatHeader = reader.ReadUInt32();
                        entries.Add(new FloatKeyFrame(reader.ReadSingle(), reader.ReadSingle()));
                    }

                    uint controlDataHeader = reader.ReadUInt32();
                    if (controlDataHeader == FloatKeyFrame.StartControlPointHeader)
                    {
                        while (reader.Peek<uint>() == FloatKeyFrame.ControlPointHeader)
                        {
                            uint floatControlHeader = reader.ReadUInt32();
                            controlPoints.Add(new FloatKeyFrame(reader.ReadSingle(), reader.ReadSingle()));
                        }

                        uint endControlPointHeader = reader.ReadUInt32();
                        if (endControlPointHeader != TrackKeyFrame.EndControlPointHeader)
                            throw new UnexpectedDataException("Expected " + TrackKeyFrame.EndControlPointHeader.ToString("X") +
                                " at end of float control points, found " + endControlPointHeader.ToString("X"));
                    }
                } else if (dataHeader == Vector3KeyFrame.EntryHeader)
                {
                    while (reader.Peek<uint>() == Vector3KeyFrame.EntryHeader)
                    {
                        uint vector3Header = reader.ReadUInt32();
                        entries.Add(new Vector3KeyFrame(reader.ReadSingle(), reader.ReadVector3()));
                    }

                    uint controlDataHeader = reader.ReadUInt32();
                    if (controlDataHeader == Vector3KeyFrame.StartControlPointHeader)
                    {
                        while (reader.Peek<uint>() == Vector3KeyFrame.ControlPointHeader)
                        {
                            uint vector3ControlHeader = reader.ReadUInt32();
                            controlPoints.Add(new Vector3KeyFrame(reader.ReadSingle(), reader.ReadVector3()));
                        }

                        uint endControlPointHeader = reader.ReadUInt32();
                        if (endControlPointHeader != TrackKeyFrame.EndControlPointHeader)
                            throw new UnexpectedDataException("Expected " + TrackKeyFrame.EndControlPointHeader.ToString("X") +
                                " at end of vector control points, found " + endControlPointHeader.ToString("X"));
                    }
                } else
                {
                    throw new InvalidDataException("Unknown track data header " + dataHeader);
                }

                tracks.Add(new Track(trackType, length, trackDim, trackMode, interpolationType,
                    evaluationType, entries, controlPoints));
            }

            return tracks;
        }

        private NodeLink ReadNodeLink(BinaryReader reader)
        {
            uint nodeLinkHeader = reader.ReadUInt32();
            if (nodeLinkHeader != NodeLink.Header)
                throw new UnexpectedDataException("Expected " + NodeLink.Header.ToString("X") +
                    " as node link header, found " + nodeLinkHeader.ToString("X"));
            uint bitfield = reader.ReadUInt32();
            if (bitfield != 1 && bitfield != 3 && bitfield != 2)
                throw new UnexpectedDataException("Expected node link bit field 1, 2 or 3, found " + bitfield);
            BfString parent = reader.ReadBfString();
            BfString slot = reader.ReadBfString();
            BfString destinationSlot = reader.ReadBfString();

            uint world = reader.ReadUInt32();
            uint node = reader.ReadUInt32();
            uint floor = reader.ReadUInt32();
            uint aim = reader.ReadUInt32();
            uint span = reader.ReadUInt32();

            uint locator = 0;
            if (bitfield > 2)
                locator = reader.ReadUInt32();
            return new NodeLink(parent, slot, destinationSlot, world, node, floor, aim, span, bitfield, locator);
        }

        private List<IStaticTrack> ReadStaticTracks(BinaryReader reader)
        {
            List<IStaticTrack> tracks = new List<IStaticTrack>();

            while (reader.Peek<uint>() == StaticTrack.Header)
            {
                uint trackHeader = reader.ReadUInt32();
                int one = reader.ReadInt32();
                if (one != 1)
                    throw new UnexpectedDataException("Expected one in static track, found " + one);
                StaticTrackType trackType = reader.ReadEnumValue<StaticTrackType>();

                uint dataHeader = reader.ReadUInt32();
                switch (dataHeader)
                {
                    case FloatStaticTrack.FloatHeader:
                        tracks.Add(new FloatStaticTrack(trackType, reader.ReadSingle()));
                        break;
                    case Vector3StaticTrack.Vector3Header:
                        tracks.Add(new Vector3StaticTrack(trackType, reader.ReadVector3()));
                        break;
                    case StringStaticTrack.StringHeader:
                        tracks.Add(new StringStaticTrack(trackType, reader.ReadBfString()));
                        break;
                    case Vector3OtherStaticTrack.Vector3OtherHeader:
                        throw new NotImplementedException(); // No special effect seems to use this static track
                        tracks.Add(new Vector3OtherStaticTrack(trackType, reader.ReadVector3()));
                        break;
                    default:
                        throw new UnexpectedDataException("Unknown static data track header " + dataHeader);
                }
            }

            return tracks;
        }
    }
}

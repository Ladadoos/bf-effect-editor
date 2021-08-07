// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Application.Commands;
using BattleForgeEffectEditor.Application.ViewModel.StaticTrack;
using BattleForgeEffectEditor.Models;
using BattleForgeEffectEditor.Models.Elements;
using BattleForgeEffectEditor.Models.Enums;
using BattleForgeEffectEditor.Models.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace BattleForgeEffectEditor.Application.ViewModel
{
    public class StaticTrackListViewModel : ObservableObject
    {
        public ObservableCollection<IStaticTrackViewModel> StaticTracks { get; set; }

        public ICommand AddStaticTrackCommand => new RelayCommand((_) => AddStaticTrack());

        public IEnumerable<StaticTrackType> StaticTracksEnums
        {
            get
            {
                // TODO Not all elements support all tracks.
                return Enum.GetValues(typeof(StaticTrackType))
                           .OfType<StaticTrackType>()
                           .Where(s => !Element?.StaticTracks.Any(s2 => s2.TrackType == s) ?? true);
            }
        }

        private StaticTrackType selectedStaticTrackType;
        public StaticTrackType SelectedStaticTrackType
        {
            get => selectedStaticTrackType;
            set
            {
                selectedStaticTrackType = value;
                OnPropertyChanged();
            }
        }

        public bool AddStaticTrackButtonEnabled => StaticTracksEnums.Count() > 0;

        public IElement Element { get; set; }

        public StaticTrackListViewModel()
        {
            StaticTracks = new ObservableCollection<IStaticTrackViewModel>();
        }

        public void Update(IElement element)
        {
            Element = element;
            StaticTracks.Clear();
            CreateListItems();

            UpdateAddStaticTrackControls();
        }

        private void AddStaticTrack()
        {
            IStaticTrack staticTrack = CreateStaticTrackFromType(selectedStaticTrackType);
            IStaticTrackViewModel staticTrackViewModel = CreateViewModelForStaticTrack(staticTrack);

            Element.StaticTracks.Add(staticTrack);
            StaticTracks.Add(staticTrackViewModel);

            UpdateAddStaticTrackControls();
        }

        public void RemoveStaticTrack<T>(StaticTrackViewModel<T> staticTrack) where T : IStaticTrack
        {
            StaticTracks.Remove(staticTrack);
            Element.StaticTracks.Remove(staticTrack.Track);

            UpdateAddStaticTrackControls();
        }

        public void UpdateAddStaticTrackControls()
        {
            RaisePropertyChanged(() => StaticTracksEnums);
            RaisePropertyChanged(() => AddStaticTrackButtonEnabled);
            if (AddStaticTrackButtonEnabled)
                SelectedStaticTrackType = StaticTracksEnums.First();
        }

        private void CreateListItems()
        {
            foreach (IStaticTrack staticTrack in Element.StaticTracks)
                StaticTracks.Add(CreateViewModelForStaticTrack(staticTrack));
        }

        private IStaticTrack CreateStaticTrackFromType(StaticTrackType trackType)
        {
            switch (trackType)
            {
                case StaticTrackType.Unknown:
                    return new StringStaticTrack(trackType, new BfString());
                case StaticTrackType.FXVersion:
                case StaticTrackType.Offset_Rotation:
                    return new Vector3StaticTrack(trackType, Vector3.Zero);
                default:
                    return new FloatStaticTrack(trackType, 0);
            }
        }

        private IStaticTrackViewModel CreateViewModelForStaticTrack(IStaticTrack track)
        {
            switch (track.TrackType)
            {
                case StaticTrackType.Priority:
                    return new PriorityStaticTrackViewModel(this, track);
                case StaticTrackType.SoundLoop:
                    return new SoundLoopStaticTrackViewModel(this, track);
                case StaticTrackType.Effect_Type:
                    return new EffectTypeStaticTrackViewModel(this, track);
                case StaticTrackType.Unknown:
                    return new StringStaticTrackViewModel(this, track);
                case StaticTrackType.Manual_BCube:
                case StaticTrackType.Fade_on_start:
                case StaticTrackType.Fade_on_kill:
                case StaticTrackType.Manual_sort_order:
                    return new BooleanStaticTrackViewModel(this, track);
                case StaticTrackType.FXVersion:
                case StaticTrackType.Offset_Rotation:
                    return new Vector3StaticTrackViewModel(this, track);
                default:
                    return new FloatStaticTrackViewModel(this, track);
            }
        }
    }
}

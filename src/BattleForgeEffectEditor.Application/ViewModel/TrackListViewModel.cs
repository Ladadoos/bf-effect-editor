// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Application.Commands;
using BattleForgeEffectEditor.Models;
using BattleForgeEffectEditor.Models.Elements;
using BattleForgeEffectEditor.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace BattleForgeEffectEditor.Application.ViewModel
{
    public class TrackListViewModel : ObservableObject
    {
        public ICommand AddTrackCommand => new RelayCommand((_) => AddTrack());

        public ObservableCollection<TrackListItemViewModel> Tracks { get; set; }

        private TrackListItemViewModel selectedItem;
        public TrackListItemViewModel SelectedItem
        {
            get => selectedItem;
            set
            {
                if (selectedItem != null)
                    selectedItem.IsSelected = false;

                if (value != null)
                    value.IsSelected = true;

                selectedItem = value;

                if (value != null)
                {
                    elementEditor.DynamicTrackEditor = new DynamicTrackEditorViewModel(selectedItem.Track);
                    TrackLastSelectionMapper[Element] = selectedItem.Track;
                }
                elementEditor.UpdateDynamicTrackEditorVisiblity();
            }
        }

        public IEnumerable<TrackType> TracksEnums
        {
            get
            {
                // TODO Not all elements support all tracks.
                var enums = Enum.GetValues(typeof(TrackType)).OfType<TrackType>();
                if (string.IsNullOrEmpty(searchFieldText))
                    return enums;
                return enums.Where(t => t.ToString().ToLower().Contains(searchFieldText.ToLower()));
            }
        }

        private TrackType selectedTrackType;
        public TrackType SelectedTrackType
        {
            get => selectedTrackType;
            set
            {
                selectedTrackType = value;
                OnPropertyChanged();
            }
        }

        public bool AddTrackButtonEnabled => TracksEnums.Count() > 0;

        private string searchFieldText = string.Empty;
        public string SearchFieldText
        {
            get => searchFieldText;
            set
            {
                searchFieldText = value;
                OnPropertyChanged();
                UpdateAddTrackControls();
            }
        }

        private IElement Element { get; set; }

        private Dictionary<IElement, Track> TrackLastSelectionMapper = new Dictionary<IElement, Track>();

        private ElementEditorViewModel elementEditor;

        public TrackListViewModel(ElementEditorViewModel elementEditor)
        {
            this.elementEditor = elementEditor;
            Tracks = new ObservableCollection<TrackListItemViewModel>();
        }

        public void Update(IElement element)
        {
            Element = element;
            Tracks.Clear();
            CreateListItems();

            UpdateAddTrackControls();
        }

        private void AddTrack()
        {
            Track track = new Track(SelectedTrackType);

            Element.Tracks.Add(track);
            Tracks.Add(new TrackListItemViewModel(this, track));

            UpdateAddTrackControls();
        }

        public void RemoveTrack(TrackListItemViewModel track)
        {
            Tracks.Remove(track);
            Element.Tracks.Remove(track.Track);

            UpdateAddTrackControls();
        }

        public void UpdateAddTrackControls()
        {
            RaisePropertyChanged(() => TracksEnums);
            RaisePropertyChanged(() => AddTrackButtonEnabled);

            if (AddTrackButtonEnabled)
                SelectedTrackType = TracksEnums.First();
        }

        private void CreateListItems()
        {
            TrackLastSelectionMapper.TryGetValue(Element, out Track lastTrack);

            foreach (Track track in Element.Tracks)
            {
                TrackListItemViewModel trackItem = new TrackListItemViewModel(this, track);
                Tracks.Add(trackItem);

                if (track == lastTrack)
                    SelectedItem = trackItem;
            }
        }
    }
}

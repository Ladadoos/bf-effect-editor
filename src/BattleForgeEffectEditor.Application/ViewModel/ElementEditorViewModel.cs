// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using BattleForgeEffectEditor.Application.ViewModel.ElementSettings;
using BattleForgeEffectEditor.Models;
using BattleForgeEffectEditor.Models.Elements;
using System;
using System.Collections.Generic;

namespace BattleForgeEffectEditor.Application.ViewModel
{
    public class ElementEditorViewModel : ObservableObject
    {
        public StaticTrackListViewModel StaticTracksList { get; private set; }

        public TrackListViewModel TracksList { get; private set; }

        public NodeLinkViewModel NodeLink { get; private set; }

        private DynamicTrackEditorViewModel dynamicTrackEditor;
        public DynamicTrackEditorViewModel DynamicTrackEditor
        {
            get => dynamicTrackEditor;
            set
            {
                dynamicTrackEditor = value;
                OnPropertyChanged();
            }
        }

        public bool ShowDynamicTrackEditor => TracksList.SelectedItem != null;

        private IElementSettings elementSettings;
        public IElementSettings ElementSettings
        {
            get => elementSettings;
            set
            {
                elementSettings = value;
                OnPropertyChanged();
            }
        }

        private IElement element;

        public ElementEditorViewModel()
        {
            StaticTracksList = new StaticTrackListViewModel();
            TracksList = new TrackListViewModel(this);
            NodeLink = new NodeLinkViewModel();
        }

        public void Update(ElementTreeItemViewModel treeElement)
        {
            element = treeElement.Element;

            StaticTracksList.Update(element);
            TracksList.Update(element);
            NodeLink.Update(element);

            ElementSettings = GetElementSettingsViewModel(treeElement);
        }

        public void UpdateDynamicTrackEditorVisiblity() => RaisePropertyChanged(() => ShowDynamicTrackEditor);

        private IElementSettings GetElementSettingsViewModel(ElementTreeItemViewModel treeElement)
        {
            var @switch = new Dictionary<Type, Func<IElementSettings>> {
                { typeof(AnimatedMesh), () => { return new AnimatedMeshSettingsViewModel(treeElement); } },
                { typeof(Billboard), () => { return new BillBoardSettingsViewModel(treeElement); } },
                { typeof(CameraShake), () => { return new EmptySettingsViewModel(treeElement); } },
                { typeof(Decal), () => { return new DecalSettingsViewModel(treeElement); } },
                { typeof(Effect), () => { return new EffectSettingsViewModel(treeElement); } },
                { typeof(Emitter), () => { return new EmitterSettingsViewModel(treeElement); } },
                { typeof(Force), () => { return new EmptySettingsViewModel(treeElement); } },
                { typeof(ForcePoint), () => { return new EmptySettingsViewModel(treeElement); } },
                { typeof(Light), () => { return new LightSettingsViewModel(treeElement); } },
                { typeof(Mesh), () => { return new MeshSettingsViewModel(treeElement); } },
                { typeof(Physic), () => { return new PhysicSettingsViewModel(treeElement); } },
                { typeof(PhysicGroup), () => { return new EmptySettingsViewModel(treeElement); } },
                { typeof(SfpEmitter), () => { return new EmptySettingsViewModel(treeElement); } },
                { typeof(SfpForceField), () => { return new EmptySettingsViewModel(treeElement); } },
                { typeof(SfpSystem), () => { return new SfpSystemSettingsViewModel(treeElement); } },
                { typeof(Sound), () => { return new SoundSettingsViewModel(treeElement); } },
                { typeof(StaticDecal), () => { return new StaticDecalSettingsViewModel(treeElement); } },
                { typeof(Trail), () => { return new TrailSettingsViewModel(treeElement); } },
                { typeof(WaterDecal), () => { return new WaterDecalSettingsViewModel(treeElement); } },
                { typeof(SpecialEffect), () => { return new SpecialEffectSettingsViewModel(treeElement); } },
            };
            return @switch[treeElement.Element.GetType()]();
        }
    }
}

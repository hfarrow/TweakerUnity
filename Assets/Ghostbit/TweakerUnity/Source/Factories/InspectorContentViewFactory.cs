using System;
using System.Collections.Generic;
using Ghostbit.Tweaker.Core;
using UnityEngine;

namespace Ghostbit.Tweaker.UI
{
	public class InspectorContentViewFactory
	{
		private InspectorView inspectorView;
		private ITweakerLogger logger = LogManager.GetCurrentClassLogger();
		private Color errorColor = new Color(1.0f, 0.5f, 0.5f);
		private Color successColor;

		public InspectorContentViewFactory(InspectorView inspectorView)
		{
			this.inspectorView = inspectorView;
			successColor = inspectorView.StringSmallEditPrefab.InputText.targetGraphic.color;
		}

		public IEnumerable<IInspectorContentView> MakeContentViews(ITweakable tweakable)
		{
			if (tweakable.HasToggle)
			{
				// TODO: show toggles
			}
			else if (tweakable.TweakableType.IsEnum)
			{
				// TODO: show enum values (same view as toggles);
			}
			else if (tweakable.TweakableType == typeof(string))
			{
				yield return MakeEditStringView(tweakable);
			}
			else if(tweakable.TweakableType == typeof(bool))
			{
				yield return MakeEditBoolView(tweakable);
			}
			else if(tweakable.TweakableType.IsNumericType())
			{
				yield return MakeEditNumericView(tweakable);
				// TODO: Show constraints as slider
				// TODO: show stepper if tweakable.IsStep
			}
			else
			{
				yield return null;
				// TODO: use input text and attempt json deserialization to tweakable.TweakableType
			}
		}

		public IInspectorContentView MakeEditStringView(ITweakable tweakable)
		{
			InspectorStringView stringView = inspectorView.InstantiateInspectorComponent(inspectorView.StringEditPrefab);
			stringView.InputText.targetGraphic.color = successColor;
			stringView.InputText.text = tweakable.GetValue().ToString();
			stringView.ValueChanged += (newValue) =>
			{
				tweakable.SetValue(newValue);
			};
			stringView.gameObject.SetActive(true);
			return stringView;
		}

		public IInspectorContentView MakeEditNumericView(ITweakable tweakable)
		{
			InspectorStringView stringView = inspectorView.InstantiateInspectorComponent(inspectorView.StringSmallEditPrefab);
			stringView.InputText.text = tweakable.GetValue().ToString();
			stringView.ValueChanged += (newValue) =>
			{
				object value = null;

				// First attempt to parse the input into a reasonably large type.
				// For example: If the input is int64 but the tweakable is int32, the input will be
				// converted (possibly truncated) to int32.
				Int64 int64;
				double doub;
				decimal dec;
				if(Int64.TryParse(newValue, out int64))
				{
					value = int64;
				}
				else if (double.TryParse(newValue, out doub))
				{
					value = doub;
				}
				else if (decimal.TryParse(newValue, out dec))
				{
					value = dec;
				}

				// If parsing was succesful, attempt the conversion.
				// If parsing or conversion failed, make the input text background errorColor.
				if(value == null)
				{
					logger.Warn("Failed to parse string to numeric type: {0}", newValue);
					stringView.InputText.targetGraphic.color = errorColor;
					//stringView.InputText.text = tweakable.GetValue().ToString();
				}
				else
				{
					object convertedValue = Convert.ChangeType(value, tweakable.TweakableType);
					if (convertedValue == null)
					{
						logger.Warn("Failed to convert value '{0}' of type {1} to tweakable of type {2}.", value.ToString(), value.GetType().FullName, tweakable.TweakableType.FullName);
						stringView.InputText.targetGraphic.color = errorColor;
						//stringView.InputText.text = tweakable.GetValue().ToString();
					}
					else
					{
						tweakable.SetValue(convertedValue);
						stringView.InputText.targetGraphic.color = successColor;

						// In case the converted number was rounded or the tweakable is constrained.
						stringView.InputText.text = tweakable.GetValue().ToString();
					}
				}
			};
			stringView.gameObject.SetActive(true);
			return stringView;
		}

		public IInspectorContentView MakeEditBoolView(ITweakable tweakable)
		{
			InspectorBoolView boolView = inspectorView.InstantiateInspectorComponent(inspectorView.BoolEditPrefab);
			bool value = (bool)tweakable.GetValue();
			boolView.Toggle.isOn = value;
			boolView.ToggleText.text = value.ToString();
			boolView.ValueChanged += (newValue) =>
			{
				tweakable.SetValue(newValue);
				boolView.ToggleText.text = newValue.ToString();
			};
			boolView.gameObject.SetActive(true);
			return boolView;
		}
	}
}

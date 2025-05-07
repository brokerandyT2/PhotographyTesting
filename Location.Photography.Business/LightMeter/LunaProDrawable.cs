using Location.Photography.Shared.ExposureCalculator;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;


namespace Location.Photography.Business.LightMeter
{

    public class LunaProDrawable : IDrawable
    {
        private GraphicsView meter;

        // Current angle values for each dial
        private float outerDialAngle = 0f; // ASA/ISO dial
        private float middleDialAngle = 0f; // Shutter speed dial
        private float innerDialAngle = 0f; // F-stop dial

        // Currently selected value indices
        private int selectedAsaIndex = 0;
        private int selectedShutterSpeedIndex = 0;
        private int selectedFStopIndex = 0;

        // Value arrays
        private readonly string[] asaValues = { "1,2", "1,5", "2", "3", "4", "5", "6", "8", "10", "15", "22", "30", "50", "100", "250" };
        private readonly string[] shutterSpeeds = { "1", "1,5", "2", "4", "8", "15", "30", "60", "125", "250", "500" };
        private readonly string[] fStops = { "1,4", "2", "2,8", "4", "5,6", "8", "11", "16" };

        // Dragging state
        private bool isDraggingOuterDial = false;
        private bool isDraggingMiddleDial = false;
        private bool isDraggingInnerDial = false;
        private PointF lastDragPoint;

        // Store dial dimensions for hit testing
        private float dialCenterX;
        private float dialCenterY;
        private float outerDialRadius;
        private float middleDialRadius;
        private float innerDialRadius;

        public LunaProDrawable(GraphicsView meter)
        {
            this.meter = meter;

            // Set up touch handling using PanGestureRecognizer
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            meter.GestureRecognizers.Add(panGesture);

            // Add tap gesture recognizer for direct selection
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += OnTapped;
            meter.GestureRecognizers.Add(tapGesture);
        }

        public LunaProDrawable() { }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            // Set up dimensions - use portrait orientation
            float width = dirtyRect.Width;
            float height = dirtyRect.Height;
            float centerX = width / 2;

            // Create more portrait-like aspect ratio
            height = width * 1.6f; // Make height greater than width for portrait orientation

            // Draw the outer case (dark gray rectangular body with more pronounced corners)
            canvas.FillColor = Color.FromRgb(45, 45, 45);
            canvas.FillRoundedRectangle(width * 0.05f, height * 0.03f, width * 0.9f, height * 0.94f, 30);

            // Add inner border for depth
            canvas.StrokeColor = Color.FromRgb(30, 30, 30);
            canvas.StrokeSize = 3;
            canvas.DrawRoundedRectangle(width * 0.08f, height * 0.05f, width * 0.84f, height * 0.9f, 25);

            // Draw sections
            DrawMeterDisplay(canvas, width, height);
            DrawDialSystem(canvas, width, height);
            DrawBottomLabel(canvas, width, height);
        }

        private void DrawMeterDisplay(ICanvas canvas, float width, float height)
        {
            float centerX = width / 2;
            float meterTop = height * 0.08f;
            float meterHeight = height * 0.25f;
            float meterWidth = width * 0.8f;

            // Draw meter background with off-white/cream color
            canvas.FillColor = Color.FromRgb(240, 235, 220);
            canvas.FillRoundedRectangle(centerX - meterWidth / 2, meterTop, meterWidth, meterHeight, 10);

            // Draw meter border
            canvas.StrokeColor = Color.FromRgb(30, 30, 30);
            canvas.StrokeSize = 3;
            canvas.DrawRoundedRectangle(centerX - meterWidth / 2, meterTop, meterWidth, meterHeight, 10);

            // Draw the meter scale
            float meterCenterY = (meterTop + meterHeight / 2) + 70;
            float scaleWidth = meterWidth;
            float arcRadius = meterHeight;

            // Draw arc for scale
            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 2;
            float startAngle = 150 * (float)(Math.PI / 180); // Adjusted to match reference
            float endAngle = 30 * (float)(Math.PI / 180);

            // Draw tick marks and labels for -3 to +3 scale
            canvas.StrokeSize = 1.5f;
            canvas.FontSize = 16;
            canvas.FontColor = Colors.Black;

            string[] values = { "-5", "-4", "-3", "-2", "-1", "0", "1", "2", "3", "4", "5" };
            for (int i = 0; i < values.Length; i++)
            {
                float angle = startAngle + i * (endAngle - startAngle) / (values.Length - 1);

                // Draw all tick marks
                float tickLength = 10;
                float innerRadius = arcRadius - 2;
                float outerRadius = arcRadius;

                float innerX = centerX + innerRadius * (float)Math.Cos(angle);
                float innerY = meterCenterY - innerRadius * (float)Math.Sin(angle) + meterHeight * 0.1f;
                float outerX = centerX + outerRadius * (float)Math.Cos(angle);
                float outerY = meterCenterY - outerRadius * (float)Math.Sin(angle) + meterHeight * 0.1f;

                canvas.DrawLine(innerX, innerY, outerX, outerY);

                // Draw additional smaller tick marks between main values
                if (i < values.Length - 1)
                {
                    for (int j = 1; j < 5; j++)
                    {
                        float subAngle = angle + j * (((endAngle - startAngle) / (values.Length - 1)) / 5);
                        float subTickLength = (j == 5 / 2) ? 7 : 5; // Medium tick in middle

                        float subInnerRadius = arcRadius - subTickLength;
                        float subOuterRadius = arcRadius;

                        float subInnerX = centerX + subInnerRadius * (float)Math.Cos(subAngle);
                        float subInnerY = meterCenterY - subInnerRadius * (float)Math.Sin(subAngle) + meterHeight * 0.1f;
                        float subOuterX = centerX + subOuterRadius * (float)Math.Cos(subAngle);
                        float subOuterY = meterCenterY - subOuterRadius * (float)Math.Sin(subAngle) + meterHeight * 0.1f;

                        canvas.DrawLine(subInnerX, subInnerY, subOuterX, subOuterY);
                    }
                }

                // Draw the value labels
                float textRadius = arcRadius - 25;
                float textX = centerX + textRadius * (float)Math.Cos(angle);
                float textY = meterCenterY - textRadius * (float)Math.Sin(angle) + meterHeight * 0.1f;

                canvas.DrawString(values[i], textX, textY, HorizontalAlignment.Center);
            }

            // Draw "EV" label in the center
            canvas.FontSize = 24;
            canvas.FontColor = Colors.Black;
            var x = new Microsoft.Maui.Graphics.Font(Microsoft.Maui.Graphics.Font.Default.ToString(), 24, FontStyleType.Italic | FontStyleType.Normal);
            canvas.Font = x;

            canvas.DrawString("EV", centerX, meterCenterY + (meterHeight - 110) * 0.15f, HorizontalAlignment.Center);
            canvas.Font = null;

            // Draw the meter needle
            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = 2;
            float needleAngle = 30 * (float)(Math.PI / 180.0); // Position at +3 on the scale
            float needleLength = arcRadius - 10;

            float needleEndX = centerX + needleLength * (float)Math.Cos(needleAngle);
            float needleEndY = meterCenterY - needleLength * (float)Math.Sin(needleAngle) + meterHeight * 0.1f;

            canvas.DrawLine(centerX, meterCenterY + (meterHeight - 110) * 0.1f, needleEndX, needleEndY);

            // Draw pivot point circle
            canvas.FillColor = Colors.Red;
            canvas.FillCircle(centerX, meterCenterY + meterHeight * 0.1f, 3);
        }

        private void DrawDialSystem(ICanvas canvas, float width, float height)
        {
            float centerX = width / 2;
            float dialY = height * 0.5f + 60;

            // Store for hit testing
            dialCenterX = centerX;
            dialCenterY = dialY;

            outerDialRadius = width * 0.38f;
            middleDialRadius = outerDialRadius * 0.7f;
            innerDialRadius = middleDialRadius * 0.65f;

            // Draw outer dial (ASA/ISO) with dark background
            canvas.FillColor = Color.FromRgb(112, 112, 112);
            canvas.FillCircle(centerX, dialY, outerDialRadius);

            // Apply rotation to the outer dial by using save/restore
            canvas.SaveState();
            canvas.Translate(centerX, dialY);
            canvas.Rotate(-outerDialAngle * 180 / (float)Math.PI);
            canvas.Translate(-centerX, -dialY);

            // Draw ASA values and markings on outer dial
            canvas.StrokeColor = Color.FromRgb(200, 190, 150); // Gold/cream color
            canvas.FontColor = Color.FromRgb(200, 190, 150);
            canvas.FontSize = 14;

            for (int i = 0; i < asaValues.Length; i++)
            {
                float angle = i * (float)(2 * Math.PI / asaValues.Length);
                float innerRadius = outerDialRadius - 15;
                float outerRadius = outerDialRadius - 5;

                float x1 = centerX + innerRadius * (float)Math.Sin(angle);
                float y1 = dialY - innerRadius * (float)Math.Cos(angle);
                float x2 = centerX + outerRadius * (float)Math.Sin(angle);
                float y2 = dialY - outerRadius * (float)Math.Cos(angle);

                canvas.DrawLine(x1, y1, x2, y2);

                // Draw ASA values
                float textRadius = outerDialRadius - 25;
                float textX = centerX + textRadius * (float)Math.Sin(angle);
                float textY = dialY - textRadius * (float)Math.Cos(angle);

                canvas.DrawString(asaValues[i], textX, textY, HorizontalAlignment.Center);
            }

            // Draw indicator marker for selected ASA value
            canvas.FillColor = Color.FromRgb(240, 120, 30); // Orange indicator
            float markerX = centerX;
            float markerY = dialY - outerDialRadius + 15;
            canvas.FillCircle(markerX, markerY, 5);

            canvas.RestoreState();

            // Draw "ASA" label
            canvas.FontSize = 18;
            var x = new Microsoft.Maui.Graphics.Font(Microsoft.Maui.Graphics.Font.Default.ToString(), 18, FontStyleType.Normal);
            canvas.Font = x;
            canvas.Font = null;

            // Middle dial (shutter speed) with teal/green color
            canvas.FillColor = Color.FromRgb(45, 90, 85); // Teal/green color
            canvas.FillCircle(centerX, dialY, middleDialRadius);

            // Apply rotation to the middle dial
            canvas.SaveState();
            canvas.Translate(centerX, dialY);
            canvas.Rotate(-middleDialAngle * 180 / (float)Math.PI);
            canvas.Translate(-centerX, -dialY);

            // Draw highlighted section (for sync speed area)
            float highlightStartAngle = 240 * (float)(Math.PI / 180); // Adjust as needed
            float highlightEndAngle = 290 * (float)(Math.PI / 180);   // Adjust as needed

            canvas.FillColor = Color.FromRgb(180, 120, 40); // Orange/gold color
            canvas.FillArc(centerX - middleDialRadius, dialY - middleDialRadius,
                          middleDialRadius * 2, middleDialRadius * 2,
                          highlightStartAngle, highlightEndAngle - highlightStartAngle, true);

            // Draw shutter speed markings
            canvas.StrokeColor = Color.FromRgb(200, 190, 150);
            canvas.FontColor = Color.FromRgb(200, 190, 150);
            canvas.FontSize = 12;

            for (int i = 0; i < shutterSpeeds.Length; i++)
            {
                float angle = i * (float)(2 * Math.PI / shutterSpeeds.Length);
                float textRadius = middleDialRadius - 18;
                float tickInnerRadius = middleDialRadius - 10;
                float tickOuterRadius = middleDialRadius - 3;

                // Draw tick mark
                float x1 = centerX + tickInnerRadius * (float)Math.Sin(angle);
                float y1 = dialY - tickInnerRadius * (float)Math.Cos(angle);
                float x2 = centerX + tickOuterRadius * (float)Math.Sin(angle);
                float y2 = dialY - tickOuterRadius * (float)Math.Cos(angle);

                canvas.DrawLine(x1, y1, x2, y2);

                // Draw shutter speed value
                float textX = centerX + textRadius * (float)Math.Sin(angle);
                float textY = dialY - textRadius * (float)Math.Cos(angle);

                canvas.DrawString(shutterSpeeds[i], textX, textY, HorizontalAlignment.Center);
            }

            // Draw indicator marker for selected shutter speed
            canvas.FillColor = Color.FromRgb(240, 120, 30); // Orange indicator
            markerX = centerX;
            markerY = dialY - middleDialRadius + 15;
            canvas.FillCircle(markerX, markerY, 4);

            canvas.RestoreState();

            // Inner dial (F-stops) - dark center with teal ring
            canvas.FillColor = Color.FromRgb(25, 80, 70); // Darker teal
            canvas.FillCircle(centerX, dialY, innerDialRadius);

            // Apply rotation to the inner dial
            canvas.SaveState();
            canvas.Translate(centerX, dialY);
            canvas.Rotate(-innerDialAngle * 180 / (float)Math.PI);
            canvas.Translate(-centerX, -dialY);

            // Draw f-stop markings
            canvas.StrokeColor = Color.FromRgb(200, 190, 150);
            canvas.FontColor = Color.FromRgb(200, 190, 150);
            canvas.FontSize = 12;

            for (int i = 0; i < fStops.Length; i++)
            {
                float angle = i * (float)(2 * Math.PI / fStops.Length);
                float textRadius = innerDialRadius - 18;
                float tickInnerRadius = innerDialRadius - 10;
                float tickOuterRadius = innerDialRadius - 3;

                // Draw tick mark
                float x1 = centerX + tickInnerRadius * (float)Math.Sin(angle);
                float y1 = dialY - tickInnerRadius * (float)Math.Cos(angle);
                float x2 = centerX + tickOuterRadius * (float)Math.Sin(angle);
                float y2 = dialY - tickOuterRadius * (float)Math.Cos(angle);

                canvas.DrawLine(x1, y1, x2, y2);

                // Draw f-stop value without "f/" prefix
                float textX = centerX + textRadius * (float)Math.Sin(angle);
                float textY = dialY - textRadius * (float)Math.Cos(angle);
                canvas.DrawString(fStops[i], textX, textY, HorizontalAlignment.Center);
            }

            // Draw indicator marker for selected f-stop
            canvas.FillColor = Color.FromRgb(240, 120, 30); // Orange indicator
            markerX = centerX;
            markerY = dialY - innerDialRadius + 15;
            canvas.FillCircle(markerX, markerY, 3);

            // Draw small "f" on each side of inner dial
            canvas.FontSize = 14;
            canvas.DrawString("f", centerX + innerDialRadius - 20, dialY, HorizontalAlignment.Center);
            canvas.DrawString("f", centerX - innerDialRadius + 20, dialY, HorizontalAlignment.Center);

            canvas.RestoreState();

            // Central black hub with stylized "f"
            float hubRadius = innerDialRadius * 0.5f;
            canvas.FillColor = Color.FromRgb(30, 30, 30); // Very dark gray/black
            canvas.FillCircle(centerX, dialY, hubRadius);

            // Draw stylized "f" in center
            canvas.FontSize = 28;
            var xx = new Microsoft.Maui.Graphics.Font(Microsoft.Maui.Graphics.Font.Default.ToString(), 28, FontStyleType.Italic | FontStyleType.Normal);
            canvas.Font = xx;
            canvas.DrawString("ƒ", centerX, dialY - 5, HorizontalAlignment.Center);
            canvas.Font = null;
        }

        private void DrawBottomLabel(ICanvas canvas, float width, float height)
        {
            float centerX = width / 2;
            float labelY = height * 0.87f;
            float labelWidth = width * 0.8f;
            float labelHeight = height * 0.08f;

            // Draw the label background
            canvas.FillColor = Color.FromRgb(45, 45, 45);
            canvas.FillRoundedRectangle(centerX - labelWidth / 2, labelY, labelWidth, labelHeight, 10);

            // Draw the "GOSSEN LUNA-PRO" text
            canvas.FontSize = 18;
            canvas.FontColor = Color.FromRgb(200, 190, 150); // Gold/cream color
            var x = new Microsoft.Maui.Graphics.Font(Microsoft.Maui.Graphics.Font.Default.ToString(), 18, FontStyleType.Normal);
            canvas.Font = x;

            canvas.DrawString("PixMap-PRO", centerX, labelY + labelHeight / 2 - 2, HorizontalAlignment.Center);

            // Display current selected values
            canvas.FontSize = 14;
            string displayValues = $"{asaValues[selectedAsaIndex]} • {shutterSpeeds[selectedShutterSpeedIndex]} • f/{fStops[selectedFStopIndex]}";
            canvas.DrawString(displayValues, centerX, labelY + labelHeight / 2 + 20, HorizontalAlignment.Center);

            canvas.Font = null;
        }

        // Tap gesture handling
        private void OnTapped(object sender, TappedEventArgs e)
        {
            // Get the touch point
            Point touchPoint = (Point)e.GetPosition(meter);
            if (touchPoint == null)
                return;

            // Calculate distance from touch point to dial center
            float dx = (float)touchPoint.X - dialCenterX;
            float dy = (float)touchPoint.Y - dialCenterY;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);

            // Determine which dial was tapped
            if (distance <= outerDialRadius && distance > middleDialRadius)
            {
                // Handle tap on outer dial (ASA/ISO)
                float angle = (float)Math.Atan2(dy, dx);
                // Convert to positive angle (0 to 2π)
                if (angle < 0) angle += (float)(2 * Math.PI);
                // Adjust angle to match dial orientation (clockwise, starting from top)
                angle = (float)(Math.PI / 2) - angle;
                if (angle < 0) angle += (float)(2 * Math.PI);

                // Calculate nearest value
                float stepAngle = (float)(2 * Math.PI / asaValues.Length);
                int nearestIndex = (int)Math.Round(angle / stepAngle) % asaValues.Length;

                // Update dial angle and selection
                outerDialAngle = nearestIndex * stepAngle;
                selectedAsaIndex = nearestIndex;

                // Request redraw
                meter?.Invalidate();
            }
            else if (distance <= middleDialRadius && distance > innerDialRadius)
            {
                // Handle tap on middle dial (shutter speed)
                float angle = (float)Math.Atan2(dy, dx);
                if (angle < 0) angle += (float)(2 * Math.PI);
                angle = (float)(Math.PI / 2) - angle;
                if (angle < 0) angle += (float)(2 * Math.PI);

                float stepAngle = (float)(2 * Math.PI / shutterSpeeds.Length);
                int nearestIndex = (int)Math.Round(angle / stepAngle) % shutterSpeeds.Length;

                middleDialAngle = nearestIndex * stepAngle;
                selectedShutterSpeedIndex = nearestIndex;

                meter?.Invalidate();
            }
            else if (distance <= innerDialRadius && distance > innerDialRadius * 0.5f)
            {
                // Handle tap on inner dial (f-stop)
                float angle = (float)Math.Atan2(dy, dx);
                if (angle < 0) angle += (float)(2 * Math.PI);
                angle = (float)(Math.PI / 2) - angle;
                if (angle < 0) angle += (float)(2 * Math.PI);

                float stepAngle = (float)(2 * Math.PI / fStops.Length);
                int nearestIndex = (int)Math.Round(angle / stepAngle) % fStops.Length;

                innerDialAngle = nearestIndex * stepAngle;
                selectedFStopIndex = nearestIndex;

                meter?.Invalidate();
            }
        }
        private void HandlePanUpdated(PanUpdatedEventArgs e)
        {
            // Update our tracking of the latest pan event
            lastPanEventArgs = e;

            // Skip if we're not dragging any dial
            if (!isDraggingOuterDial && !isDraggingMiddleDial && !isDraggingInnerDial)
                return;

            // Get the current touch position
            Point touchPoint = GetCurrentTouchPosition();

            // Calculate the new angle based on the touch point
            float newAngle = CalculateAngle((float)touchPoint.X, (float)touchPoint.Y);

            // Apply the angle to the appropriate dial
            if (isDraggingOuterDial)
            {
                outerDialAngle = newAngle;
            }
            else if (isDraggingMiddleDial)
            {
                middleDialAngle = newAngle;
            }
            else if (isDraggingInnerDial)
            {
                innerDialAngle = newAngle;
            }

            // Save the current point for future calculations
            lastDragPoint = new PointF((float)touchPoint.X, (float)touchPoint.Y);

            // Request a redraw of the control
            meter.Invalidate();
        }
        private float CalculateAngle(float x, float y)
        {
            // Calculate the angle between the center of the dial and the touch point
            float dx = x - dialCenterX;
            float dy = y - dialCenterY;

            // Calculate the angle in radians
            float angle = (float)Math.Atan2(dy, dx);

            // Convert to degrees
            angle = angle * 180f / (float)Math.PI;

            // Adjust to standard angle measurement (0-360 degrees, clockwise from the top)
            angle = (angle + 90) % 360;

            // Ensure the angle is positive
            if (angle < 0)
                angle += 360;

            return angle;
        }
        // Pan gesture handling
        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    HandlePanStart(e);
                    break;

                case GestureStatus.Running:
                    HandlePanRunning(e);
                    break;

                case GestureStatus.Completed:
                case GestureStatus.Canceled:
                    isDraggingOuterDial = false;
                    isDraggingMiddleDial = false;
                    isDraggingInnerDial = false;
                    break;
            }
        }

        private void HandlePanStart(PanUpdatedEventArgs e)
        {
            // Store the current pan event arguments for position tracking
            lastPanEventArgs = e;

            // Initialize the starting position reference point
            // For a pan start, we don't have a direct way to get position
            // Instead, we'll use a default position and track relative movement
            // This is typically the center of your control
            initialTouchPosition = new Point(dialCenterX, dialCenterY);

            // IMPORTANT: We don't use e.GetPosition() because it doesn't exist
            // Instead, we use our custom method
            Point touchPoint = GetCurrentTouchPosition();

            // Calculate distance from touch point to dial center
            float dx = (float)touchPoint.X - dialCenterX;
            float dy = (float)touchPoint.Y - dialCenterY;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);

            // Determine which dial was touched based on distance
            if (distance <= outerDialRadius && distance > middleDialRadius)
            {
                isDraggingOuterDial = true;
                isDraggingMiddleDial = false;
                isDraggingInnerDial = false;
            }
            else if (distance <= middleDialRadius && distance > innerDialRadius)
            {
                isDraggingOuterDial = false;
                isDraggingMiddleDial = true;
                isDraggingInnerDial = false;
            }
            else if (distance <= innerDialRadius && distance > innerDialRadius * 0.5f)
            {
                isDraggingOuterDial = false;
                isDraggingMiddleDial = false;
                isDraggingInnerDial = true;
            }

            // Save the current point for angle calculations
            lastDragPoint = new PointF((float)touchPoint.X, (float)touchPoint.Y);
        }

        private Point GetCurrentTouchPosition()
        {
            // Calculate the touch position using the PanUpdatedEventArgs relative data
            // and a known reference point (like the dial center)

            if (lastPanEventArgs != null)
            {
                // If we have a fixed starting point (initialTouchPosition)
                if (initialTouchPosition != null)
                {
                    // Calculate current position as initial position + total movement
                    return new Point(
                        initialTouchPosition.Value.X + lastPanEventArgs.TotalX,
                        initialTouchPosition.Value.Y + lastPanEventArgs.TotalY
                    );
                }

                // If no initial position is available, use the dial center as reference
                return new Point(
                    dialCenterX + lastPanEventArgs.TotalX,
                    dialCenterY + lastPanEventArgs.TotalY
                );
            }

            // Fallback: If no touch data is available, return dial center
            return new Point(dialCenterX, dialCenterY);
        }

        private PanUpdatedEventArgs lastPanEventArgs;
        private Point? initialTouchPosition;
        private void HandlePanRunning(PanUpdatedEventArgs e)
        {
            if (!isDraggingOuterDial && !isDraggingMiddleDial && !isDraggingInnerDial)
                return;

            // Get current touch position
            Point currentTouchPoint = GetCurrentTouchPosition();
            if (currentTouchPoint == null)
                return;

            PointF currentPoint = new PointF((float)currentTouchPoint.X, (float)currentTouchPoint.Y);

            // Calculate angle between last point and current point relative to dial center
            float angleDelta = CalculateAngleDelta(lastDragPoint, currentPoint);

            // Apply rotation to the appropriate dial
            if (isDraggingOuterDial)
            {
                outerDialAngle -= angleDelta; // Negative for clockwise rotation
                SnapOuterDialToValue();
            }
            else if (isDraggingMiddleDial)
            {
                middleDialAngle -= angleDelta;
                SnapMiddleDialToValue();
            }
            else if (isDraggingInnerDial)
            {
                innerDialAngle -= angleDelta;
                SnapInnerDialToValue();
            }

            // Update last drag point
            lastDragPoint = currentPoint;

            // Request redraw
            meter?.Invalidate();
        }

        private float CalculateAngleDelta(PointF point1, PointF point2)
        {
            // Calculate angles of both points relative to dial center
            float angle1 = (float)Math.Atan2(point1.Y - dialCenterY, point1.X - dialCenterX);
            float angle2 = (float)Math.Atan2(point2.Y - dialCenterY, point2.X - dialCenterX);

            // Calculate the difference
            float delta = angle2 - angle1;

            // Handle wraparound
            if (delta > Math.PI) delta -= (float)(2 * Math.PI);
            if (delta < -Math.PI) delta += (float)(2 * Math.PI);

            return delta;
        }

        // Methods to snap dials to nearest value
        private void SnapOuterDialToValue()
        {
            // Calculate the step angle for ASA values
            float stepAngle = (float)(2 * Math.PI / asaValues.Length);

            // Normalize the angle to 0-2π range
            while (outerDialAngle < 0) outerDialAngle += (float)(2 * Math.PI);
            while (outerDialAngle >= 2 * Math.PI) outerDialAngle -= (float)(2 * Math.PI);

            // Calculate the closest index
            int closestIndex = (int)Math.Round(outerDialAngle / stepAngle);
            closestIndex = (closestIndex % asaValues.Length + asaValues.Length) % asaValues.Length;

            // Snap to closest value
            outerDialAngle = closestIndex * stepAngle;
            selectedAsaIndex = closestIndex;
        }

        private void SnapMiddleDialToValue()
        {
            // Calculate the step angle for shutter speeds
            float stepAngle = (float)(2 * Math.PI / shutterSpeeds.Length);

            // Normalize the angle
            while (middleDialAngle < 0) middleDialAngle += (float)(2 * Math.PI);
            while (middleDialAngle >= 2 * Math.PI) middleDialAngle -= (float)(2 * Math.PI);

            // Calculate the closest index
            int closestIndex = (int)Math.Round(middleDialAngle / stepAngle);
            closestIndex = (closestIndex % shutterSpeeds.Length + shutterSpeeds.Length) % shutterSpeeds.Length;

            // Snap to closest value
            middleDialAngle = closestIndex * stepAngle;
            selectedShutterSpeedIndex = closestIndex;
        }

        private void SnapInnerDialToValue()
        {
            // Calculate the step angle for f-stops
            float stepAngle = (float)(2 * Math.PI / fStops.Length);

            // Normalize the angle
            while (innerDialAngle < 0) innerDialAngle += (float)(2 * Math.PI);
            while (innerDialAngle >= 2 * Math.PI) innerDialAngle -= (float)(2 * Math.PI);

            // Calculate the closest index
            int closestIndex = (int)Math.Round(innerDialAngle / stepAngle);
            closestIndex = (closestIndex % fStops.Length + fStops.Length) % fStops.Length;

            // Snap to closest value
            innerDialAngle = closestIndex * stepAngle;
            selectedFStopIndex = closestIndex;
        }

        // Public property to get the current selected values
        public (string Asa, string ShutterSpeed, string FStop) SelectedValues
        {
            get
            {
                return (
                    asaValues[selectedAsaIndex],
                    shutterSpeeds[selectedShutterSpeedIndex],
                    fStops[selectedFStopIndex]
                );
            }
        }
    }//end of class

}
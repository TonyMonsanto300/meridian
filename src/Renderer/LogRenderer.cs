using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; // For input handling
using System;
using System.Collections.Generic;
using XenWorld.Config;
using XenWorld.src.Repository.GUI;

public class LogRenderer {
    private SpriteBatch spriteBatch;
    private Texture2D whiteTexture;
    private Texture2D blackTexture;
    private SpriteFont font;
    private static List<string> logMessages = new List<string>();

    private int maxMessages; // Maximum number of messages to display per page
    private float lineHeight; // Height of each line in pixels (float for precision)
    private Rectangle logWindowArea; // The area of the log window (adjusted width)
    private Rectangle paginationBarArea; // The area of the pagination bar
    private int currentMessageIndex; // Index of the first message to display on the current page
    private bool isAtLatestMessages; // Tracks if we're viewing the latest messages

    // For input handling
    private MouseState previousMouseState;

    private int lastLogMessagesCount; // To track if new messages have been added

    public LogRenderer(SpriteBatch spriteBatch, SpriteFont font) {
        this.spriteBatch = spriteBatch;
        this.whiteTexture = TextureDictionary.Context["whiteTexture"]; // White background
        this.blackTexture = TextureDictionary.Context["blackTexture"]; // Black borders and lines
        this.font = font;

        // Calculate the unused area dimensions once
        int unusedAreaX = 0;
        int unusedAreaY = RenderConfig.MapViewPortY * RenderConfig.CellSize;
        int unusedAreaWidth = RenderConfig.MapViewPortX * RenderConfig.CellSize;
        int unusedAreaHeight = (RenderConfig.WindowSizeY - RenderConfig.MapViewPortY) * RenderConfig.CellSize;

        // Set the width of the pagination bar
        int paginationBarWidth = RenderConfig.CellSize;

        // Adjust the log window area to account for the pagination bar
        int logWindowWidth = unusedAreaWidth - paginationBarWidth;
        logWindowArea = new Rectangle(unusedAreaX, unusedAreaY, logWindowWidth, unusedAreaHeight);

        // Set up the pagination bar area
        paginationBarArea = new Rectangle(unusedAreaX + logWindowWidth, unusedAreaY, paginationBarWidth, unusedAreaHeight);

        // Set the maximum number of messages to display per page
        maxMessages = 10;

        // Calculate the line height as a float
        lineHeight = (float)unusedAreaHeight / maxMessages;

        // Initialize the current message index to display the most recent messages
        currentMessageIndex = Math.Max(0, logMessages.Count - maxMessages);

        // Initialize to viewing the latest messages
        isAtLatestMessages = true;

        previousMouseState = Mouse.GetState();

        lastLogMessagesCount = logMessages.Count;
    }

    public void DrawLog() {
        // Handle input and update logic within DrawLog
        MouseState mouseState = Mouse.GetState();

        // Detect a mouse click (button released this frame after being pressed)
        if (mouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed) {
            Point mousePosition = new Point(mouseState.X, mouseState.Y);

            // Check if the mouse click is within the pagination bar
            if (paginationBarArea.Contains(mousePosition)) {
                // Height of the top and bottom control boxes
                int controlBoxHeight = RenderConfig.CellSize;

                // Define rectangles for the control boxes
                Rectangle topBox = new Rectangle(
                    paginationBarArea.X,
                    paginationBarArea.Y,
                    paginationBarArea.Width,
                    controlBoxHeight);

                Rectangle bottomBox = new Rectangle(
                    paginationBarArea.X,
                    paginationBarArea.Y + paginationBarArea.Height - controlBoxHeight,
                    paginationBarArea.Width,
                    controlBoxHeight);

                Rectangle middleBox = new Rectangle(
                    paginationBarArea.X,
                    topBox.Y + topBox.Height,
                    paginationBarArea.Width,
                    paginationBarArea.Height - 2 * controlBoxHeight);

                if (topBox.Contains(mousePosition)) {
                    // Navigate back by 10 messages
                    NavigateBack();
                } else if (middleBox.Contains(mousePosition)) {
                    // Reset to the most recent messages
                    ResetToLatest();
                } else if (bottomBox.Contains(mousePosition)) {
                    // Navigate forward by 10 messages
                    NavigateForward();
                }
            }
        }

        previousMouseState = mouseState;

        // Check if new messages have been added
        if (logMessages.Count != lastLogMessagesCount) {
            if (isAtLatestMessages) {
                currentMessageIndex = Math.Max(0, logMessages.Count - maxMessages);
            }
            lastLogMessagesCount = logMessages.Count;
        }

        // Draw the white background for the log window
        spriteBatch.Draw(
            whiteTexture,
            logWindowArea,
            Color.White);

        // Draw the white background for the pagination bar
        spriteBatch.Draw(
            whiteTexture,
            paginationBarArea,
            Color.Gray); // You can choose a different color

        // Draw borders for the log window (top, bottom, left)
        int borderThickness = 1;

        // Top border
        spriteBatch.Draw(
            blackTexture,
            new Rectangle(logWindowArea.X, logWindowArea.Y, logWindowArea.Width, borderThickness),
            Color.Black);

        // Bottom border
        spriteBatch.Draw(
            blackTexture,
            new Rectangle(logWindowArea.X, logWindowArea.Y + logWindowArea.Height - borderThickness, logWindowArea.Width, borderThickness),
            Color.Black);

        // Left border
        spriteBatch.Draw(
            blackTexture,
            new Rectangle(logWindowArea.X, logWindowArea.Y, borderThickness, logWindowArea.Height),
            Color.Black);

        // Draw messages based on the current page
        int messageCount = logMessages.Count;
        int messagesToDisplay = Math.Min(maxMessages, messageCount - currentMessageIndex);

        for (int i = 0; i < messagesToDisplay; i++) {
            int messageIndex = currentMessageIndex + i;
            if (messageIndex >= messageCount)
                break; // Safety check

            string message = logMessages[messageIndex];

            // Calculate Y position for each message
            float yPos = logWindowArea.Y + i * lineHeight;

            // Set the text position with horizontal padding
            Vector2 position = new Vector2(logWindowArea.X + 5, yPos + (lineHeight - font.LineSpacing) / 2f);

            // Draw the message
            spriteBatch.DrawString(font, message, position, Color.Black);

            // Draw a black line below each message
            float lineY = yPos + lineHeight - borderThickness / 2f;
            spriteBatch.Draw(
                blackTexture,
                new Rectangle(logWindowArea.X, (int)Math.Round(lineY), logWindowArea.Width, borderThickness),
                Color.Black);
        }

        // Draw pagination controls
        DrawPaginationControls();
    }

    private void DrawPaginationControls() {
        // Height of the top and bottom control boxes
        int controlBoxHeight = RenderConfig.CellSize;

        // Define rectangles for the three control boxes
        Rectangle topBox = new Rectangle(
            paginationBarArea.X,
            paginationBarArea.Y,
            paginationBarArea.Width,
            controlBoxHeight);

        Rectangle bottomBox = new Rectangle(
            paginationBarArea.X,
            paginationBarArea.Y + paginationBarArea.Height - controlBoxHeight,
            paginationBarArea.Width,
            controlBoxHeight);

        Rectangle middleBox = new Rectangle(
            paginationBarArea.X,
            topBox.Y + topBox.Height,
            paginationBarArea.Width,
            paginationBarArea.Height - 2 * controlBoxHeight);

        // Draw the boxes (you can choose different colors)
        spriteBatch.Draw(whiteTexture, topBox, Color.LightGray);
        spriteBatch.Draw(whiteTexture, middleBox, Color.Gray);
        spriteBatch.Draw(whiteTexture, bottomBox, Color.DarkGray);

        // Draw symbols or text to indicate functionality
        DrawStringCentered("^", topBox);
        DrawStringCentered("X", middleBox);
        DrawStringCentered("v", bottomBox);
    }

    // Helper method to draw text centered in a rectangle
    private void DrawStringCentered(string text, Rectangle area) {
        Vector2 textSize = font.MeasureString(text);
        Vector2 position = new Vector2(
            area.X + (area.Width - textSize.X) / 2f,
            area.Y + (area.Height - textSize.Y) / 2f);

        spriteBatch.DrawString(font, text, position, Color.Black);
    }

    private void NavigateBack() {
        // Move back by maxMessages
        currentMessageIndex = Math.Max(0, currentMessageIndex - maxMessages);
        isAtLatestMessages = currentMessageIndex == logMessages.Count - maxMessages;
    }

    private void NavigateForward() {
        // Move forward by maxMessages
        currentMessageIndex = Math.Min(logMessages.Count - maxMessages, currentMessageIndex + maxMessages);
        isAtLatestMessages = currentMessageIndex == logMessages.Count - maxMessages;
    }

    private void ResetToLatest() {
        // Reset to the most recent messages
        currentMessageIndex = Math.Max(0, logMessages.Count - maxMessages);
        isAtLatestMessages = true;
    }

    public static void AddLogMessage(string message) {
        // Add the new message to the list
        logMessages.Add(message);
    }
}
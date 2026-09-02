# BRichTextBox

Extended WinForms `RichTextBox` with colored log lines, exception helpers, auto-scroll and optional timestamps.

## Features

- `AppendLine(text, color)` — append colored text
- `AppendErr(text)` — append red error line
- `AppendExc(exception)` — append exception message (red)
- `AutoScrollToBottom` — auto-scroll to latest line
- `AddDate` — prefix each line with current date/time
- `SuspendPainting` / `ResumePainting` — flicker-free batch updates

## Targets

.NET 4.0, .NET Framework 4.6.2, .NET 8 (Windows), .NET 10 (Windows)

## License

MIT

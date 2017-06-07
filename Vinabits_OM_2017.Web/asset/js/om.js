function HightlightOnGotFocus(s, e) {
    ChangeBgColor(s, 'Yellow');
}
function FadeOnLostFocus(s, e) {
    ChangeBgColor(s, 'White');
}
function ChangeBgColor(edit, color) {
    edit.GetMainElement().style.backgroundColor = color;
    edit.GetInputElement().style.backgroundColor = color;
}
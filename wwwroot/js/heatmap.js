let frames = [];
let frameIndex = 0;
let playing = true;

function loadHeatmapData(csvPath, liveMode) {

    fetch(csvPath)
        .then(res => res.text())
        .then(text => {

            let lines = text.trim().split("\n");
            let frameSize = 32;

            for (let i = 0; i < lines.length; i += frameSize) {
                let frame = [];
                for (let r = 0; r < frameSize; r++) {
                    let row = lines[i + r].split(",").map(Number);
                    frame.push(row);
                }
                frames.push(frame);
            }

            if (frames.length > 0) {
                setupCanvas(liveMode);
            }
        });
}

function setupCanvas(liveMode) {
    let canvas = document.getElementById("heatmapCanvas");
    let ctx = canvas.getContext("2d");

    function render() {
        ctx.clearRect(0, 0, canvas.width, canvas.height);

        let frame = frames[frameIndex];

        let cellW = canvas.width / 32;
        let cellH = canvas.height / 32;

        for (let r = 0; r < 32; r++) {
            for (let c = 0; c < 32; c++) {

                let value = frame[r][c];

                // Color mapping (blue → yellow → red)
                let color = getColor(value);

                ctx.fillStyle = color;
                ctx.fillRect(c * cellW, r * cellH, cellW, cellH);
            }
        }

        if (liveMode && playing) {
            frameIndex = (frameIndex + 1) % frames.length;
            requestAnimationFrame(render);
        }
    }

    if (!liveMode) {
        let slider = document.getElementById("frameSlider");
        slider.max = frames.length - 1;
        slider.oninput = () => {
            frameIndex = parseInt(slider.value);
            render();
        };
    }

    render();
}

function getColor(value) {
    if (value < 20) return "#64b5f6";      // blue
    if (value < 40) return "#81c784";      // green
    if (value < 70) return "#fff176";      // yellow
    return "#e57373";                      // red
}

<template>
  <canvas ref="canvas"
          @mousemove="draw"
          @mousedown="setPosition"
          @mouseenter="setPosition"
          @resize="resize"
          id="canvas"></canvas>
</template>

<script>
export default {
  name: "App",
  data() {
    return {
      pos: {},
    };
  },
  methods: {
    setPosition(e) {
      this.pos.x = e.clientX;
      this.pos.y = e.clientY;
    },
    resize() {
      const { canvas } = this.$refs;
      const ctx = canvas.getContext("2d");
      ctx.canvas.width = window.innerWidth;
      ctx.canvas.height = window.innerHeight;
    },
    draw(e) {
      if (e.buttons !== 1) {
        return;
      }
      const { canvas } = this.$refs;
      const ctx = canvas.getContext("2d");
      const { pos } = this;
      ctx.beginPath();
      ctx.lineWidth = 5;
      ctx.lineCap = "round";
      ctx.strokeStyle = "green";
      ctx.moveTo(pos.x, pos.y);
      this.setPosition(e);
      ctx.lineTo(pos.x, pos.y);
      ctx.stroke();
    },
  },
};
</script>

<style scoped>
  #canvas {
    border: 1px solid black;
  }
</style>

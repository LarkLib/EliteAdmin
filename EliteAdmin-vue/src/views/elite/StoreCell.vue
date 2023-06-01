<template>
  <div>
    width: {{ width }}
    height: {{ height }}
    <v-stage ref="stage" :config="configKonva" @dragstart="handleDragstart" @dragend="handleDragend">
      <v-layer ref="layer">
        <v-star v-for="item in list" :key="item.id" :config="{
          x: item.x,
          y: item.y,
          rotation: item.rotation,
          id: item.id,
          numPoints: 5,
          innerRadius: 30,
          outerRadius: 50,
          //fill: '#89b717',
          fill: 'red',
          opacity: 0.8,
          draggable: true,
          scaleX: dragItemId === item.id ? item.scale * 1.2 : item.scale,
          scaleY: dragItemId === item.id ? item.scale * 1.2 : item.scale,
          shadowColor: 'black',
          shadowBlur: 10,
          shadowOffsetX: dragItemId === item.id ? 15 : 5,
          shadowOffsetY: dragItemId === item.id ? 15 : 5,
          shadowOpacity: 0.6
        }"></v-star>
      </v-layer>
      <v-layer ref="layer">
        <v-label :config="labelConfig">
          <v-tag :config="tagConfig" />
          <v-text :config="textConfig" />
        </v-label>
      </v-layer>
    </v-stage>
  </div>
</template>

<script setup lang="js">
  import { reactive, ref } from 'vue'

  const width = window.innerWidth;
  const height = window.innerHeight;
  let vm = {};

  const list = []
  const configKonva = {
    width: width - 100,
    height: height - 120
  }
  const labelConfig = {
    x: 170,
    y: 75,
    opacity: 0.75,
    visible: false,
  }
  const tagConfig = {
    fill: 'black',
    pointerDirection: 'down',
    pointerWidth: 10,
    pointerHeight: 10,
    lineJoin: 'round',
    shadowColor: 'black',
    shadowBlur: 10,
    shadowOffset: 10,
    shadowOpacity: 0.5
  }
  const textConfig = {
    text: 'Tooltip pointing down',
    fontFamily: 'Calibri',
    fontSize: 18,
    padding: 5,
    fill: 'white'
  }
  ///////////////////////////////////////////
  //star
  let dragItemId = -1
  function handleDragstart(e) {
    // save drag element:
    dragItemId = e.target.id();
    // move current element to the top:
    const item = list.find(i => i.id === dragItemId);
    const index = list.indexOf(item);
    list.splice(index, 1);
    list.push(item);
  }
  function handleDragend(e) {
    dragItemId = null;
  }

  function generateList() {
    for (let n = 0; n < 100; n++) {
      list.push({
        id: Math.round(Math.random() * 10000).toString(),
        x: Math.random() * width,
        y: Math.random() * height,
        rotation: Math.random() * 180,
        scale: Math.random()
      });
    }
  }
  generateList()
</script>

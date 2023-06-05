<template>
  <div>
    <!-- 工具区域 -->
    <el-row :gutter="10" class="mb8">
      <el-col :span="1.5">
        <el-text type="primary">排:</el-text>
        <el-select v-model="storeQueryParams.conditions[0].fieldValue" @change="getStoreList()" placeholder="请选择排数" style="width:80px">
          <el-option v-for="item in options.sql_storeline"
                     :key="item.dictValue"
                     :label="item.dictLabel"
                     :value="item.dictValue"></el-option>
        </el-select>
      </el-col>
      <el-col :xs="8" :sm="6" :md="4" :lg="3" :xl="1">
        <el-text type="primary">货位编码:{{currentCellName}}</el-text>
      </el-col>
      <el-col :span="15.5">
        <el-button-group class="ml-14">
          <el-tooltip class="item" effect="dark" content="刷新" placement="top">
            <el-button :icon="Edit" />
          </el-tooltip>
          <el-button type="primary" :icon="Search">Search</el-button>
          <el-button type="primary" :icon="Share" />
          <el-button type="primary" :icon="Delete" />
          <el-button>Upload<el-icon class="el-icon--right"><Upload /></el-icon></el-button>
          <el-button :icon="Search" circle />
          <el-button type="primary" :icon="Edit" circle />
          <el-button type="success" :icon="Check" circle />
          <el-button type="info" :icon="Message" circle />
          <el-button type="warning" :icon="Star" circle />
          <el-button type="danger" :icon="Delete" circle />
        </el-button-group>
      </el-col>
    </el-row>

    <!--货位信息-->
    <div id="stagediv">
      <v-stage ref="stage" :config="configKonva" @dragstart="handleDragstart" @dragend="handleDragend">
        <v-layer ref="layer">
          <v-group v-for="(item,index) in storeDataList" :config="{x: 0, y: 0}">
            <!--层数坐标-->
            <v-text v-if="isWriteCellLevel(item.f_CellLevel)" :config="{
                  x:10,
                  y:320-35*(item.f_CellLevel-1),
                  text: item.f_CellLevel,
                  fontFamily: 'Calibri' ,
                  fontSize: 18,
                  padding: 5,
                  fill: '#191970'
                  }"></v-text>
            <!--列数坐标-->
            <v-text v-if="isWriteCellCol(item.f_CellCol)"
                    :config="{
                    x:15+35*item.f_CellCol,
                    y:350,
                    text: item.f_CellCol,
                    fontFamily: 'Calibri' ,
                    fontSize: 18,
                    padding: 5,
                    fill: '#191970'
                    }"></v-text>
            <!--货位-->
            <v-rect @mousemove="handleMouseMove" @mouseout="handleMouseOut" @click="handleMouseClick"
                    :config="{
                    x: 10+35*item.f_CellCol,
                    y: 320-35*(item.f_CellLevel-1),
                    width: 30,
                    height: 30,
                    id: item.f_ID.toString(), //必需是字符串类型,才能用find搜索
                    name: item.f_CellName ,
                    fill: options.store_status.find(c=>
              c.dictValue === item.f_StoreStatus).remark,
              stroke: 'black' ,
              strokeWidth: 1,
              }">
            </v-rect>
            <!--运行状态-->
            <v-text v-if="item.f_RunStatus.toLowerCase()!='enable'" :config="{
                    x: 13+35*item.f_CellCol,
                    y: 3+35*(item.f_CellLevel-1),
                    text: item.f_RunStatus[0],
                    fontFamily: 'Calibri' ,
                    fontSize: 16,
                    padding: 5,
                    fill: '#191970'
                    }"></v-text>
          </v-group>
        </v-layer>
        <v-layer ref="layer">
          <!--tooltip-->
          <v-label :config="labelConfig" ref="cellTooltip">
            <v-tag :config="tagConfig" />
            <v-text :config="textConfig" />
          </v-label>
        </v-layer>
      </v-stage>
    </div>

    <!-- 状态区域 -->
    <v-stage :config="configKonvaInfo">
      <v-layer>
        <v-text :config="{
                  x:10,
                  y:10,
                  text: '存储状态: ',
                  fontFamily: 'Calibri' ,
                  fontSize: 12,
                  padding: 5,
                  fill: '#191970'
                  }" />
        <v-group v-for="(item,index) in options.store_status" :config="{x: 0, y: 0}">
          <v-rect :config="{
                  x: index * 65 + 70,
                  y: 12,
                  width: 15,
                  height: 15,
                  name: 'colors[i]' ,
                  fill: item.remark,
                  stroke: 'black' ,
                  strokeWidth: 0,
                  }"></v-rect>
          <v-text :config="{
                  x:index * 65 + 85,
                  y:10,
                  text: item.dictLabel,
                  fontFamily: 'Calibri' ,
                  fontSize: 12,
                  padding: 5,
                  fill: '#191970'
                  }" />
        </v-group>
      </v-layer>
      <v-layer>
        <v-text :config="{
                  x:400,
                  y:10,
                  text: '运行状态: ',
                  fontFamily: 'Calibri' ,
                  fontSize: 12,
                  padding: 5,
                  fill: '#191970'
                  }" />
        <v-group v-for="(item,index) in options.run_status" :config="{x: 0, y: 0, drawBorder: true}">
          <v-text :config="{
                  x:400 + index * 50 + 60,
                  y:10,
                  text: item.dictValue[0]+'('+item.dictLabel+')',
                  fontFamily: 'Calibri' ,
                  fontSize: 12,
                  padding: 5,
                  fill: '#191970'
                  }" />
        </v-group>
      </v-layer>
    </v-stage>

    <!-- 数据区域 -->
    <el-table :data="materialDataList"
              :key="tableKey"
              v-loading="loading"
              ref="datatable"
              border
              stripe
              highlight-current-row
              @header-dragend="handleHeaderDragend"
              @selection-change="handleSelectionChange"
              @current-change="handleCurrentChange">
      <!--@sort-change="sortChange"-->
      <template v-for="item in materialColumns">
        <el-table-column v-if="item.visible&&!!item.caption"
                         show-overflow-tooltip
                         :prop="item.name"
                         :sortable="true"
                         :label="item.caption"
                         :width="item.gridWidth"
                         :type="item.control"
                         :key="item.name"
                         :resizable="true"
                         :formatter="dataFormat"
                         :itemtype="item.dataType">
          <template #default="scope" v-if="item.gridEditable && item.control=='TextBox'">
            <el-input v-model="scope.row[item.name]" style="width: v-bind(item.gridWidth-2)px" placeholder="Please input" />
          </template>
        </el-table-column>
      </template>
    </el-table>
    <!--<pagination style="padding: 32px 16px;" class="mt10" background :total="total" v-model:page="materialQueryParams.pageNum" v-model:limit="materialQueryParams.pageSize" @pagination="getMaterialList" />-->
  </div>
</template>

<script setup lang="js">
  import { reactive, ref, onMounted } from 'vue'
  import { getFiledConfigInfo, getkeyvalueGroup, listDynamicData, addDynamicObject, getDynamicObjectById, updateDynamicObject, deleteDynamicObjec } from '@/api/dynamic/dynamic.js'
  import { Delete, Edit, Search, Share, Upload, Check, Message, Star, ArrowLeft, ArrowRight, } from '@element-plus/icons-vue'

  const msg = ref()
  const { proxy } = getCurrentInstance()
  const loading = ref(false)
  const cellTooltip = ref()
  const currentCellName = ref()
  const storeDataList = ref([])
  const cellLevels = []
  const cellCols = []
  const storeQueryParams = reactive({
    "queryCode": "T_Base_StoreCell",
    "conditions": [
      {
        "fieldName": "F_CellLine",
        "conditionalType": 0,
        "fieldValue": "1"
      }, {
        "fieldName": "F_CellType",
        "conditionalType": 0,
        "fieldValue": "Cell"
      }, {
        "fieldName": "F_WarehouseID",
        "conditionalType": 0,
        "fieldValue": "1"
      }
    ],
    "pageNum": 1,
    "pageSize": 2000,
    "totalNum": 0,
    "totalPage": 0,
    "sort": "F_CellCol, F_CellLevel",
    "sortType": "desc"
  })
  const state = reactive({
    single: true,
    multiple: true,
    form: {},
    rules: {
      //  f_Employeecode: [{ required: true, message: "工号不能为空", trigger: "blur" }],
      //  f_Employeename: [{ required: true, message: "姓名不能为空", trigger: "blur" }],
      //  f_Iseffective: [{ required: true, message: "使用状态不能为空", trigger: "change" }],
    },
    options: {
      //排数 选项列表 格式 eg:{ dictLabel: '标签', dictValue: '0'}
      sql_storeline: [],
      //可用性 选项列表 格式 eg:{ dictLabel: '标签', dictValue: '0'}
      enableflag: [],
      //存储状态 选项列表 格式 eg:{ dictLabel: '标签', dictValue: '0'}
      store_status: [],
      //运行状态 选项列表 格式 eg:{ dictLabel: '标签', dictValue: '0'}
      run_status: [],
    }
  })
  const { form, rules, options, single, multiple } = toRefs(state)
  const dictParams = [
    { dictType: "enableflag" },
    { dictType: "sql_storeline" },
    { dictType: "store_status" },
    { dictType: "run_status" },
  ]

  proxy.getDicts(dictParams).then((response) => {
    response.data.forEach((element) => {
      state.options[element.dictType] = element.list
    })
  })

  const width = window.innerWidth;
  const height = window.innerHeight;

  const configKonva = {
    width: width - 100,
    height: 400 //height - 300
  }
  const configKonvaInfo = {
    width: width - 100,
    height: 50
  }

  /*tooltip*/
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

  function isWriteCellLevel(level) {
    if (!level) return false;
    if (cellLevels.includes(level))
      return false;
    else {
      cellLevels.push(level)
      return true;
    }
  }
  function isWriteCellCol(col) {
    if (!col) return false;
    if (cellCols.includes(col))
      return false;
    else {
      cellCols.push(col)
      return true;
    }
  }
  function getStoreList() {
    if (!!currentCellName.value) {
      let rect = cellTooltip.value.getNode().getStage().findOne("." + currentCellName.value)
      console.log("rect", rect)
      if (!!rect) {
        rect.stroke('black')
        rect.strokeWidth(1)
      }
      currentCellName.value = null
    }
    loading.value = true
    listDynamicData(storeQueryParams).then(res => {
      const { code, data } = res
      if (code == 200) {
        cellCols.length = 0
        cellLevels.length = 0
        storeDataList.value = data.result
        //total.value = data.totalNum
        loading.value = false
      }
    })
  }

  /*鼠标事件*/
  function handleMouseOut(event) {
    cellTooltip.value.getNode().hide();
  }
  function handleMouseMove(event) {
    const rect = event.target
    const toolTipLabel = cellTooltip.value.getNode();
    const mousePos = rect.getStage().getPointerPosition();
    msg.value = 'rect id: ' + rect.id() + ',x: ' + mousePos.x + ', y: ' + mousePos.y;
    toolTipLabel.position({
      x: mousePos.x,
      y: mousePos.y,
    });
    toolTipLabel
      .getText()
      //.text('rect id: ' + rect.id() + '\r\n color: ' + rect.fill());
      .text(rect.name());
    //toolTipLabel.moveToTop()
    toolTipLabel.show()
    event.cancelBubble = true;
  }
  function handleMouseClick(event) {
    msg.value = 'click rect, id=' + event.target.id();
    if (!!currentCellName.value) {
      let rect = event.target.getStage().findOne("." + currentCellName.value)
      if (!!rect) {
        rect.stroke('black')
        rect.strokeWidth(1)
      }
    }
    event.target.stroke('red')
    event.target.strokeWidth(3)
    currentCellName.value = event.target.name();
    materialQueryParams.conditions[0].fieldValue = event.target.id()
    getMaterialList();
  }

  getStoreList()

  onMounted(() => {
  })
  //////////
  //material
  import useUserStore from '@/store/modules/user'
  const materialTableName = "v_Base_InventoryList"
  const materialDataList = ref([])
  const materialQueryParams = reactive({
    "queryCode": materialTableName,
    "conditions": [
      {
        "fieldName": "F_StoreCell",
        "conditionalType": 0,
        "fieldValue": ""
      }
    ],
    "pageNum": 1,
    "pageSize": 50,
    "totalNum": 0,
    "totalPage": 0,
    "sort": "F_PalletCode",
    "sortType": "asc"
  })
  const materialColumns = ref([])
  const materialKeyvalueGroup = ref([])
  const tableKey = 1
  const userId = useUserStore().userId;
  const defaultProps = {
    children: 'children',
    label: 'caption'
  }

  function getTableInfo() {
    getFiledConfigInfo(materialTableName, userId).then(res => {
      const { code, data } = res
      if (code == 200) {
        materialColumns.value = data
      }
    })

    getkeyvalueGroup(materialTableName).then(res => {
      const { code, data } = res
      if (code == 200) {
        materialKeyvalueGroup.value = data
      }
    })
  }
  function getMaterialList() {
    loading.value = true
    listDynamicData(materialQueryParams).then(res => {
      const { code, data } = res
      if (code == 200) {
        materialDataList.value = data.result
        //total.value = data.totalNum
        loading.value = false
      }
    })
  }
  getTableInfo()
</script>
<style scoped>
  .auto-scoll {
    overflow: auto;
  }
</style>

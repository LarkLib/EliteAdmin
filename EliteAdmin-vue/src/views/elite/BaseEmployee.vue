<!--
 * @Descripttion: (员工管理/T_Base_Employee)
 * @Author: (admin)
 * @Date: (2023-05-25)
-->
<template>
  <div>
    <el-form :model="queryParams" label-position="right" inline ref="queryRef" v-show="showSearch" @submit.prevent>
      <el-form-item label="工号" prop="conditions[0].fieldValue">
        <el-input v-model="queryParams.conditions[0].fieldValue" placeholder="请输入工号" />
      </el-form-item>
      <el-form-item label="姓名" prop="conditions[1].fieldValue">
        <el-input v-model="queryParams.conditions[1].fieldValue" placeholder="请输入姓名" />
      </el-form-item>
      <el-form-item label="使用状态" prop="conditions[2].fieldValue">
        <el-select clearable v-model="queryParams.conditions[2].fieldValue" placeholder="请选择使用状态">
          <el-option v-for="item in  options.enableflag " :key="item.dictValue" :label="item.dictLabel" :value="item.dictValue">
            <span class="fl">{{ item.dictLabel }}</span>
            <span class="fr" style="color: var(--el-text-color-secondary);">{{ item.dictValue }}</span>
          </el-option>
        </el-select>
      </el-form-item>
      <el-form-item>
        <el-button icon="search" type="primary" @click="handleQuery">{{ $t('btn.search') }}</el-button>
        <el-button icon="refresh" @click="resetQuery">{{ $t('btn.reset') }}</el-button>
      </el-form-item>
    </el-form>
    <!-- 工具区域 -->
    <el-row :gutter="10" class="mb8">
      <el-col :span="1.5">
        <el-button type="primary" v-hasPermi="['elite:tbaseemployee:add']" plain icon="plus" @click="handleAdd">
          {{ $t('btn.add') }}
        </el-button>
      </el-col>
      <el-col :span="1.5">
        <el-button type="success" :disabled="single" v-hasPermi="['elite:tbaseemployee:edit']" plain icon="edit" @click="handleUpdate">
          {{ $t('btn.edit') }}
        </el-button>
      </el-col>
      <el-col :span="1.5">
        <el-button type="danger" :disabled="multiple" v-hasPermi="['elite:tbaseemployee:delete']" plain icon="delete" @click="handleDelete">
          {{ $t('btn.delete') }}
        </el-button>
      </el-col>
      <el-col :span="1.5">
        <el-button type="warning" plain icon="download" @click="handleExport" v-hasPermi="['elite:tbaseemployee:export']">
          {{ $t('btn.export') }}
        </el-button>
      </el-col>
      <el-col :span="1.5" justify="end">
        <!--列设置按钮-->
        <el-dropdown trigger="click">
          <el-button icon="el-icon-s-operation" size="small">列设置</el-button>
          <template #dropdown>
            <el-dropdown-menu>
              <!--<span class="title">列设置</span>-->
              <el-button type="primary" v-hasPermi="['task:projectwcstask:add']" plain icon="plus" @click="eliteColumns[0].gridWidth+=20">Save</el-button>
              <el-tree draggable :data="eliteColumns" :props="defaultProps" :allow-drop="allowDrop" @node-drop="handleDrop">
                <template #default="{ node, data }">
                  <span class="tree-table-setting">
                    <el-switch v-model="data.visible"> </el-switch>
                    <span class="spanwidth"> {{ node.label }} </span>
                    <el-input style="width:50px" v-model="data.gridWidth" placeholder="Please input" />
                    <el-input style="width:50px" v-model="data.gridIndex" placeholder="Please input" />
                  </span>
                </template>
              </el-tree>
            </el-dropdown-menu>
          </template>
        </el-dropdown>
      </el-col>
      <right-toolbar v-model:showSearch="showSearch" @queryTable="getList" :columns="null"></right-toolbar>
    </el-row>

    <!-- 数据区域 -->
    <el-table :data="dataList"
              :key="tableKey"
              v-loading="loading"
              ref="employee"
              border
              stripe
              highlight-current-row
              @header-dragend="handleHeaderDragend">
      <!--@sort-change="sortChange"
    @selection-change="handleSelectionChange">-->
      <template v-for="item in eliteColumns">
        <el-table-column v-if="item.visible"
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
          <template #default="scope" v-if="item.gridEditable==='True' && item.control=='TextBox'">
            <el-input v-model="scope.row[item.name]" style="width: v-bind(item.gridWidth-2)px" placeholder="Please input" />
            <!--<span>{{ scope.row[item.name] }}</span>-->
          </template>
        </el-table-column>
      </template>
    </el-table>
    <pagination class="mt10" background :total="total" v-model:page="queryParams.pageNum" v-model:limit="queryParams.pageSize" @pagination="getList" />

    <!-- 添加或修改员工管理对话框 -->
    <el-dialog :title="title" :lock-scroll="false" v-model="open">
      <el-form ref="formRef" :model="form" :rules="rules" label-width="100px">
        <el-row :gutter="20">

          <el-col :lg="12" v-if="opertype != 1">
            <el-form-item label="编号" prop="fId">
              <el-input-number v-model.number="form.fId" controls-position="right" placeholder="请输入编号" :disabled="true" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="工号" prop="fEmployeecode">
              <el-input v-model="form.fEmployeecode" placeholder="请输入工号" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="姓名" prop="fEmployeename">
              <el-input v-model="form.fEmployeename" placeholder="请输入姓名" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="班组" prop="fTeamgroupid">
              <el-select v-model="form.fTeamgroupid" placeholder="请选择班组">
                <el-option v-for="item in options.sql_teamgroup"
                           :key="item.dictValue"
                           :label="item.dictLabel"
                           :value="parseInt(item.dictValue)"></el-option>
              </el-select>
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="使用状态" prop="fIseffective">
              <el-select v-model="form.fIseffective" placeholder="请选择使用状态">
                <el-option v-for="item in options.enableflag"
                           :key="item.dictValue"
                           :label="item.dictLabel"
                           :value="item.dictValue"></el-option>
              </el-select>
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <template #footer v-if="opertype != 3">
        <el-button text @click="cancel">{{ $t('btn.cancel') }}</el-button>
        <el-button type="primary" @click="submitForm">{{ $t('btn.submit') }}</el-button>
      </template>
    </el-dialog>
    useUserStore.userId={{useUserStore().userId}}<br />
    dataList<br />{{dataList}}<br /><br /><br />
    keyvalueGroup<br />{{keyvalueGroup}}<br /><br /><br />
    eliteColumns<br />{{eliteColumns}}
  </div>
</template>

<script setup name="baseemployee">
  import { getFiledConfigInfo, getkeyvalueGroup, listDynamicData, addDynamicObject } from '@/api/dynamic/dynamic.js'
   import useUserStore from '@/store/modules/user'

  const { proxy } = getCurrentInstance()
  const ids = ref([])
  const loading = ref(false)
  const showSearch = ref(true)

  const queryParams = reactive({
    "queryCode": "T_Base_Employee",
    "conditions": [
      {
        "fieldName": "F_Employeecode",
        "conditionalType": 0,
        "fieldValue": ""
      }, {
        "fieldName": "F_Employeename",
        "conditionalType": 1,
        "fieldValue": ""
      }, {
        "fieldName": "F_IsEffective",
        "conditionalType": 0,
        "fieldValue": ""
      }
    ],
    "pageNum": 1,
    "pageSize": 3,
    "totalNum": 0,
    "totalPage": 0,
    "sort": "F_ID",
    "sortType": "desc"
  })
  //const columns = ref([
  //  { visible: true, prop: 'fId', label: '编号' },
  //  { visible: true, prop: 'fEmployeecode', label: '工号' },
  //  { visible: true, prop: 'fEmployeename', label: '姓名' },
  //  { visible: true, prop: 'fTeamgroupid', label: '班组' },
  //  { visible: true, prop: 'fIseffective', label: '使用状态' },
  //])
  const total = ref(0)
  const dataList = ref([])
  const queryRef = ref()
  const defaultTime = ref([new Date(2000, 1, 1, 0, 0, 0), new Date(2000, 2, 1, 23, 59, 59)])

  const tableName = "T_Base_Employee"
  const eliteColumns = ref([])
  const keyvalueGroup = ref([])
  const tableKey = 1
  const userId=useUserStore().userId;

  function getTableInfo() {
    getFiledConfigInfo(tableName,userId).then(res => {
      const { code, data } = res
      if (code == 200) {
        eliteColumns.value = data
        console.log("getFiledConfigInfo data:", data)
      }
    })

    getkeyvalueGroup(tableName).then(res => {
      const { code, data } = res
      if (code == 200) {
        keyvalueGroup.value = data
        console.log("getkeyvalueGroup data:", data)
      }
    })
  }
  const defaultProps = {
    children: 'children',
    label: 'caption'
  }

  function allowDrop(draggingNode, dropNode, type) {
    // 仅允许Tree节点上下拖动
    return type !== 'inner'
  }

  function handleHeaderDragend(newWidth, oldWidth, column, event) {
    console.log("handleHeaderDragend newWidth %s, oldWidth %s", newWidth, oldWidth, "event:", event, "column:", column)
    eliteColumns.value.find(item => item.name === column.property).gridWidth = newWidth
  }

  function handleDrop(draggingNode, dropNode, dropType, ev) {
    console.log('tree drop:', dropNode.label, dropType, draggingNode)
  }

  function dataFormat(row, column, cellValue, index) {
    //console.log("dateFormat cellValue %s, index %s", cellValue, index, "row:", row, "column:", column)
    if (column.type == 'LookUpEdit') return column.property in keyvalueGroup.value && cellValue in keyvalueGroup.value[column.property] ? keyvalueGroup.value[column.property][cellValue] : cellValue
    return cellValue
  }

  var dictParams = [
    { dictType: "enableflag" },
    { dictType: "sql_teamgroup" },
  ]

  proxy.getDicts(dictParams).then((response) => {
    response.data.forEach((element) => {
      state.options[element.dictType] = element.list
    })
  })


  function getList() {
    loading.value = true
    listDynamicData(queryParams).then(res => {
      const { code, data } = res
      if (code == 200) {
        dataList.value = data.result
        total.value = data.totalNum
        loading.value = false
      }
    })
  }

  // 查询
  function handleQuery() {
    queryParams.pageNum = 1
    getList()
  }

  // 重置查询操作
  function resetQuery() {
    proxy.resetForm("queryRef")
    handleQuery()
  }
  // 多选框选中数据
  function handleSelectionChange(selection) {
    ids.value = selection.map((item) => item.fId);
    single.value = selection.length != 1
    multiple.value = !selection.length;
  }
  // 自定义排序
  function sortChange(column) {
    var sort = undefined
    var sortType = undefined

    if (column.prop != null && column.order != null) {
      sort = column.prop
      sortType = column.order

    }
    queryParams.sort = sort
    queryParams.sortType = sortType
    handleQuery()
  }

  /*************** form操作 ***************/
  const formRef = ref()
  const title = ref('')
  // 操作类型 1、add 2、edit 3、view
  const opertype = ref(0)
  const open = ref(false)
  const state = reactive({
    single: true,
    multiple: true,
    form: {},
    rules: {
      fEmployeecode: [{ required: true, message: "工号不能为空", trigger: "blur" }],
      fEmployeename: [{ required: true, message: "姓名不能为空", trigger: "blur" }],
      fIseffective: [{ required: true, message: "使用状态不能为空", trigger: "change" }],
    },
    options: {
      // 班组 选项列表 格式 eg:{ dictLabel: '标签', dictValue: '0'}
      fTeamgroupidOptions: [],
      // 使用状态 选项列表 格式 eg:{ dictLabel: '标签', dictValue: '0'}
      enableflag: [],
    }
  })

  const { form, rules, options, single, multiple } = toRefs(state)

  // 关闭dialog
  function cancel() {
    open.value = false
    reset()
  }

  // 重置表单
  function reset() {
    form.value = {
      fId: undefined,
      fEmployeecode: undefined,
      fEmployeename: undefined,
      fTeamgroupid: undefined,
      fIseffective: undefined,
    };
    proxy.resetForm("formRef")
  }

  // 添加按钮操作
  function handleAdd() {
    reset();
    open.value = true
    title.value = '添加'
    opertype.value = 1
  }

  // 修改按钮操作
  function handleUpdate(row) {
    reset()
    const id = row.fId || ids.value
    getBaseEmployee(id).then((res) => {
      const { code, data } = res
      if (code == 200) {
        open.value = true
        title.value = "修改数据"
        opertype.value = 2

        form.value = {
          ...data,
        }
      }
    })
  }

  // 添加&修改 表单提交
  function submitForm() {
    console.log("form.value", form.value)
    proxy.$refs["formRef"].validate((valid) => {
      if (valid) {
        if (form.value.fId != undefined && opertype.value === 2) {
          updateBaseEmployee(form.value).then((res) => {
            proxy.$modal.msgSuccess("修改成功")
            open.value = false
            getList()
          })
            .catch(() => { })
        } else {
          addDynamicObject(form.value).then((res) => {
            proxy.$modal.msgSuccess("新增成功")
            open.value = false
            getList()
          })
            .catch(() => { })
        }
      }
    })
  }

  // 删除按钮操作
  function handleDelete(row) {
    const Ids = row.fId || ids.value

    proxy
      .$confirm('是否确认删除参数编号为"' + Ids + '"的数据项？')
      .then(function () {
        return delBaseEmployee(Ids)
      })
      .then(() => {
        getList()
        proxy.$modal.msgSuccess("删除成功")
      })
      .catch(() => { })
  }


  /**
   * 查看
   * @param {*} row
   */
  function handlePreview(row) {
    reset()
    open.value = true
    title.value = '查看'
    opertype.value = 3
    form.value = row
  }

  // 导出按钮操作
  function handleExport() {
    proxy
      .$confirm("是否确认导出员工管理数据项?", "警告", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
      })
      .then(async () => {
        await proxy.downFile('/elite/BaseEmployee/export', { ...queryParams })
      })
  }

  getTableInfo()
//handleQuery()
</script>
<style lang="scss" scoped>
  .spanwidth {
    display: -moz-inline-box;
    display: inline-block;
    width: 60px;
  }
</style>

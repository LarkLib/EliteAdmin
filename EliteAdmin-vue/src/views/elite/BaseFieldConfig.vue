<!--
 * @Descripttion: (字段信息配置/T_Base_FieldConfig)
 * @Author: (admin)
 * @Date: (2023-05-23)
-->
<template>
  <div>
    <el-form :model="queryParams" label-position="right" inline ref="queryRef" v-show="showSearch" @submit.prevent>
      <el-form-item label="表名" prop="tableName">
        <el-input v-model="queryParams.tableName" placeholder="请输入表名" />
      </el-form-item>
      <el-form-item label="名称" prop="name">
        <el-input v-model="queryParams.name" placeholder="请输入名称" />
      </el-form-item>
      <el-form-item>
        <el-button icon="search" type="primary" @click="handleQuery">{{ $t('btn.search') }}</el-button>
        <el-button icon="refresh" @click="resetQuery">{{ $t('btn.reset') }}</el-button>
      </el-form-item>
    </el-form>
    <!-- 工具区域 -->
    <el-row :gutter="10" class="mb8">
      <el-col :span="1.5">
        <el-button type="primary" v-hasPermi="['task:basefieldconfig:add']" plain icon="plus" @click="handleAdd">
          {{ $t('btn.add') }}
        </el-button>
      </el-col>
      <el-col :span="1.5">
        <el-button type="warning" plain icon="download" @click="handleExport" v-hasPermi="['task:basefieldconfig:export']">
          {{ $t('btn.export') }}
        </el-button>
      </el-col>
      <right-toolbar v-model:showSearch="showSearch" @queryTable="getList" :columns="columns"></right-toolbar>
    </el-row>

    <!-- 数据区域 -->
    <el-table 
      :data="dataList" 
      v-loading="loading" 
      ref="table" 
      border 
      highlight-current-row 
      @sort-change="sortChange"
      >
      <el-table-column prop="id" label="编号" align="center" v-if="columns.showColumn('id')"/>
      <el-table-column prop="tableName" label="表名" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('tableName')"/>
      <el-table-column prop="name" label="名称" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('name')"/>
      <el-table-column prop="caption" label="标题" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('caption')"/>
      <el-table-column prop="gridIndex" label="序号" align="center" v-if="columns.showColumn('gridIndex')"/>
      <el-table-column prop="gridEditable" label="是否可编辑" align="center" v-if="columns.showColumn('gridEditable')"/>
      <el-table-column prop="gridWidth" label="宽度" align="center" v-if="columns.showColumn('gridWidth')"/>
      <el-table-column prop="control" label="控件" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('control')"/>
      <el-table-column prop="dataType" label="数据类型" align="center" v-if="columns.showColumn('dataType')">
        <template #default="scope">
          <dict-tag :options=" options.dataTypeOptions" :value="scope.row.dataType" />
        </template>
      </el-table-column>
      <el-table-column prop="maxLenght" label="最大长度" align="center" v-if="columns.showColumn('maxLenght')"/>
      <el-table-column prop="numPrecisionRadix" label="小数点位数" align="center" v-if="columns.showColumn('numPrecisionRadix')"/>
      <el-table-column prop="formatString" label="格式化" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('formatString')"/>
      <el-table-column prop="dataSource" label="数据源" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('dataSource')"/>
      <el-table-column prop="withBlank" label="是否有空选项" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('withBlank')"/>
      <el-table-column prop="blankValue" label="空选项默认值" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('blankValue')"/>
      <el-table-column prop="blankName" label="空选项名称" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('blankName')"/>
      <el-table-column prop="errorMessage" label="错误消息" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('errorMessage')"/>
      <el-table-column label="操作" align="center" width="160">
        <template #default="scope">
          <el-button type="primary" icon="view" @click="handlePreview(scope.row)"></el-button>
          <el-button v-hasPermi="['task:basefieldconfig:edit']" type="success" icon="edit" title="编辑" @click="handleUpdate(scope.row)"></el-button>
          <el-button v-hasPermi="['task:basefieldconfig:delete']" type="danger" icon="delete" title="删除" @click="handleDelete(scope.row)"></el-button>
        </template>
      </el-table-column>
    </el-table>
    <pagination class="mt10" background :total="total" v-model:page="queryParams.pageNum" v-model:limit="queryParams.pageSize" @pagination="getList" />

    <!-- 添加或修改字段信息配置对话框 -->
    <el-dialog :title="title" :lock-scroll="false" v-model="open" >
      <el-form ref="formRef" :model="form" :rules="rules" label-width="100px">
        <el-row :gutter="20">
        
          <el-col :lg="12" v-if="opertype != 1">
            <el-form-item label="编号" prop="id">
              <el-input-number v-model.number="form.id" controls-position="right" placeholder="请输入编号" :disabled="true"/>
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="表名" prop="tableName">
              <el-input v-model="form.tableName" placeholder="请输入表名" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="名称" prop="name">
              <el-input v-model="form.name" placeholder="请输入名称" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="标题" prop="caption">
              <el-input v-model="form.caption" placeholder="请输入标题" />
            </el-form-item>
          </el-col>
        
          <el-col :lg="12">
            <el-form-item label="序号" prop="gridIndex">
              <el-input v-model.number="form.gridIndex" placeholder="请输入序号" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="是否可编辑" prop="gridEditable">
              <el-input v-model="form.gridEditable" placeholder="请输入是否可编辑" />
            </el-form-item>
          </el-col>
        
          <el-col :lg="12">
            <el-form-item label="宽度" prop="gridWidth">
              <el-input v-model.number="form.gridWidth" placeholder="请输入宽度" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="控件" prop="control">
              <el-input v-model="form.control" placeholder="请输入控件" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="数据类型" prop="dataType">
              <el-select v-model="form.dataType" placeholder="请选择数据类型">
                <el-option 
                  v-for="item in options.dataTypeOptions" 
                  :key="item.dictValue" 
                  :label="item.dictLabel" 
                  :value="item.dictValue"></el-option>
              </el-select>
            </el-form-item>
          </el-col>
        
          <el-col :lg="12">
            <el-form-item label="最大长度" prop="maxLenght">
              <el-input v-model.number="form.maxLenght" placeholder="请输入最大长度" />
            </el-form-item>
          </el-col>
        
          <el-col :lg="12">
            <el-form-item label="小数点位数" prop="numPrecisionRadix">
              <el-input v-model.number="form.numPrecisionRadix" placeholder="请输入小数点位数" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="格式化" prop="formatString">
              <el-input v-model="form.formatString" placeholder="请输入格式化" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="验证操作" prop="validationOperator">
              <el-input v-model="form.validationOperator" placeholder="请输入验证操作" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="验证值1" prop="validationValue1">
              <el-input v-model="form.validationValue1" placeholder="请输入验证值1" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="验证值2" prop="validationValue2">
              <el-input v-model="form.validationValue2" placeholder="请输入验证值2" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="数据源" prop="dataSource">
              <el-input v-model="form.dataSource" placeholder="请输入数据源" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="是否有空选项" prop="withBlank">
              <el-input v-model="form.withBlank" placeholder="请输入是否有空选项" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="空选项默认值" prop="blankValue">
              <el-input v-model="form.blankValue" placeholder="请输入空选项默认值" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="空选项名称" prop="blankName">
              <el-input v-model="form.blankName" placeholder="请输入空选项名称" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="错误消息" prop="errorMessage">
              <el-input v-model="form.errorMessage" placeholder="请输入错误消息" />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <template #footer v-if="opertype != 3">
        <el-button text @click="cancel">{{ $t('btn.cancel') }}</el-button>
        <el-button type="primary" @click="submitForm">{{ $t('btn.submit') }}</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup name="basefieldconfig">
import { listBaseFieldConfig,
 addBaseFieldConfig, delBaseFieldConfig, 
 updateBaseFieldConfig,getBaseFieldConfig, 
 } 
from '@/api/elite/basefieldconfig.js'

const { proxy } = getCurrentInstance()
const ids = ref([])
const loading = ref(false)
const showSearch = ref(true)
const queryParams = reactive({
  pageNum: 1,
  pageSize: 10,
  sort: '',
  sortType: 'asc',
  tableName: undefined,
  name: undefined,
})
const columns = ref([
  { visible: true, prop: 'id', label: '编号' },
  { visible: true, prop: 'tableName', label: '表名' },
  { visible: true, prop: 'name', label: '名称' },
  { visible: true, prop: 'caption', label: '标题' },
  { visible: true, prop: 'gridIndex', label: '序号' },
  { visible: true, prop: 'gridEditable', label: '是否可编辑' },
  { visible: true, prop: 'gridWidth', label: '宽度' },
  { visible: true, prop: 'control', label: '控件' },
  { visible: false, prop: 'dataType', label: '数据类型' },
  { visible: false, prop: 'maxLenght', label: '最大长度' },
  { visible: false, prop: 'numPrecisionRadix', label: '小数点位数' },
  { visible: false, prop: 'formatString', label: '格式化' },
  { visible: false, prop: 'dataSource', label: '数据源' },
  { visible: false, prop: 'withBlank', label: '是否有空选项' },
  { visible: false, prop: 'blankValue', label: '空选项默认值' },
  { visible: false, prop: 'blankName', label: '空选项名称' },
  { visible: false, prop: 'errorMessage', label: '错误消息' },
])
const total = ref(0)
const dataList = ref([])
const queryRef = ref()
const defaultTime = ref([new Date(2000, 1, 1, 0, 0, 0), new Date(2000, 2, 1, 23, 59, 59)])


var dictParams = [
]


function getList(){
  loading.value = true
  listBaseFieldConfig(queryParams).then(res => {
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
function resetQuery(){
  proxy.resetForm("queryRef")
  handleQuery()
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
    tableName: [{ required: true, message: "表名不能为空", trigger: "blur" }],
    name: [{ required: true, message: "名称不能为空", trigger: "blur" }],
    control: [{ required: true, message: "控件不能为空", trigger: "blur" }],
  },
  options: {
    // 数据类型 选项列表 格式 eg:{ dictLabel: '标签', dictValue: '0'}
    dataTypeOptions: [],
  }
})

const { form, rules, options, single, multiple } = toRefs(state)

// 关闭dialog
function cancel(){
  open.value = false
  reset()
}

// 重置表单
function reset() {
  form.value = {
    id: undefined,
    tableName: undefined,
    name: undefined,
    caption: undefined,
    gridIndex: undefined,
    gridEditable: undefined,
    gridWidth: undefined,
    control: undefined,
    dataType: undefined,
    maxLenght: undefined,
    numPrecisionRadix: undefined,
    formatString: undefined,
    validationOperator: undefined,
    validationValue1: undefined,
    validationValue2: undefined,
    dataSource: undefined,
    withBlank: undefined,
    blankValue: undefined,
    blankName: undefined,
    errorMessage: undefined,
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
  const id = row.id || ids.value
  getBaseFieldConfig(id).then((res) => {
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
  proxy.$refs["formRef"].validate((valid) => {
    if (valid) {
      if (form.value.id != undefined && opertype.value === 2) {
        updateBaseFieldConfig(form.value).then((res) => {
          proxy.$modal.msgSuccess("修改成功")
          open.value = false
          getList()
        })
        .catch(() => {})
      } else {
        addBaseFieldConfig(form.value).then((res) => {
            proxy.$modal.msgSuccess("新增成功")
            open.value = false
            getList()
          })
          .catch(() => {})
      }
    }
  })
}

// 删除按钮操作
function handleDelete(row) {
  const Ids = row.id || ids.value

  proxy
    .$confirm('是否确认删除参数编号为"' + Ids + '"的数据项？')
    .then(function () {
      return delBaseFieldConfig(Ids)
    })
    .then(() => {
      getList()
      proxy.$modal.msgSuccess("删除成功")
    })
    .catch(() => {})
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
    .$confirm("是否确认导出字段信息配置数据项?", "警告", {
      confirmButtonText: "确定",
      cancelButtonText: "取消",
      type: "warning",
    })
    .then(async () => {
      await proxy.downFile('/elite/BaseFieldConfig/export', { ...queryParams })
    })
}

handleQuery()
</script>
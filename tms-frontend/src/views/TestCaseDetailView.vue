<template>
  <div class="testcase-detail-container">
    <!-- 頁首 -->
    <div class="header">
      <div class="header-left">
        <el-button @click="router.back()">← 返回</el-button>

        <div>
          <h2 class="title">
            {{ testCase?.title || '測試案例' }}
          </h2>

          <div class="subtitle">Test Case #{{ testCaseId }}</div>
        </div>
      </div>

      <div class="header-actions">
        <el-button type="primary" @click="showCreateDialog = true"> 新增 Bug </el-button>

        <el-button type="danger" plain @click="deleteTestCase"> 刪除測試案例 </el-button>
      </div>
    </div>

    <!-- Test Case 資訊 -->
    <el-card class="detail-card">
      <el-descriptions :column="2" border>
        <el-descriptions-item label="優先級">
          {{ testCase?.priority }}
        </el-descriptions-item>

        <el-descriptions-item label="狀態">
          {{ testCase?.status }}
        </el-descriptions-item>

        <el-descriptions-item label="描述" :span="2">
          {{ testCase?.description || '無描述' }}
        </el-descriptions-item>
      </el-descriptions>
    </el-card>

    <!-- Bug 區塊標題 -->
    <div class="section-header">
      <div class="section-title">
        <h3>Bug 列表</h3>
        <span class="bug-count"> 共 {{ bugs.length }} 筆 </span>
      </div>

      <el-button
        type="danger"
        :disabled="selectedBugs.length === 0"
        :loading="deletingBugs"
        @click="deleteSelectedBugs"
      >
        刪除已選取
        <span v-if="selectedBugs.length > 0"> （{{ selectedBugs.length }}） </span>
      </el-button>
    </div>

    <!-- Bug Table -->
    <el-card>
      <el-table
        :data="bugs"
        style="width: 100%"
        empty-text="目前沒有 Bug"
        @selection-change="handleSelectionChange"
      >
        <el-table-column type="selection" width="55" />

        <el-table-column prop="title" label="標題" />

        <el-table-column prop="severity" label="嚴重程度" width="120" />

        <el-table-column prop="status" label="狀態" width="120" />
      </el-table>
    </el-card>

    <!-- 新增 Bug -->
    <el-dialog v-model="showCreateDialog" title="新增 Bug" width="400px">
      <el-form :model="newBug">
        <el-form-item label="標題">
          <el-input v-model="newBug.title" />
        </el-form-item>

        <el-form-item label="描述">
          <el-input v-model="newBug.description" type="textarea" />
        </el-form-item>

        <el-form-item label="嚴重程度">
          <el-select v-model="newBug.severity">
            <el-option label="Critical" value="Critical" />
            <el-option label="High" value="High" />
            <el-option label="Medium" value="Medium" />
            <el-option label="Low" value="Low" />
          </el-select>
        </el-form-item>
      </el-form>

      <template #footer>
        <el-button @click="showCreateDialog = false"> 取消 </el-button>

        <el-button type="primary" @click="createBug"> 確認 </el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { ElMessage, ElMessageBox } from 'element-plus'
import api from '../api'

const router = useRouter()
const route = useRoute()
const testCaseId = route.params.id

const testCase = ref(null)
const bugs = ref([])

const showCreateDialog = ref(false)

const selectedBugs = ref([])
const deletingBugs = ref(false)

const newBug = ref({
  title: '',
  description: '',
  severity: 'Medium',
})

const fetchTestCase = async () => {
  try {
    const response = await api.get(`/api/testcase/${testCaseId}`)

    testCase.value = response.data
  } catch (error) {
    console.error(error)
    ElMessage.error('取得測試案例失敗')
  }
}

const fetchBugs = async () => {
  try {
    const response = await api.get(`/api/bug/testcase/${testCaseId}`)

    bugs.value = response.data
  } catch (error) {
    console.error(error)
    ElMessage.error('取得 Bug 列表失敗')
  }
}

const createBug = async () => {
  try {
    await api.post('/api/bug', {
      ...newBug.value,
      testCaseId: parseInt(testCaseId),
    })

    ElMessage.success('Bug 建立成功')

    showCreateDialog.value = false

    newBug.value = {
      title: '',
      description: '',
      severity: 'Medium',
    }

    await fetchBugs()
  } catch (error) {
    console.error(error)
    ElMessage.error('Bug 建立失敗')
  }
}

const handleSelectionChange = (rows) => {
  selectedBugs.value = rows
}

const deleteSelectedBugs = async () => {
  if (selectedBugs.value.length === 0) {
    return
  }

  try {
    await ElMessageBox.confirm(
      `確定要刪除選取的 ${selectedBugs.value.length} 筆 Bug 嗎？`,
      '刪除 Bug',
      {
        confirmButtonText: '刪除',
        cancelButtonText: '取消',
        type: 'warning',
      },
    )

    deletingBugs.value = true

    let successCount = 0
    let failedCount = 0

    for (const bug of selectedBugs.value) {
      try {
        await api.delete(`/api/bug/${bug.id}`)
        successCount++
      } catch (error) {
        console.error(`刪除 Bug ${bug.id} 失敗`, error)
        failedCount++
      }
    }

    if (failedCount === 0) {
      ElMessage.success(`成功刪除 ${successCount} 筆 Bug`)
    } else {
      ElMessage.warning(`成功 ${successCount} 筆，失敗 ${failedCount} 筆`)
    }

    selectedBugs.value = []

    await fetchBugs()
  } catch (error) {
    if (error === 'cancel' || error === 'close') {
      return
    }

    console.error(error)
    ElMessage.error('刪除 Bug 時發生錯誤')
  } finally {
    deletingBugs.value = false
  }
}

const deleteTestCase = async () => {
  try {
    await ElMessageBox.confirm(`確定要刪除「${testCase.value?.title}」嗎？`, '刪除測試案例', {
      confirmButtonText: '刪除',
      cancelButtonText: '取消',
      type: 'warning',
    })

    await api.delete(`/api/testcase/${testCaseId}`)

    ElMessage.success('測試案例刪除成功')

    router.back()
  } catch (error) {
    if (error === 'cancel' || error === 'close') {
      return
    }

    console.error(error)

    ElMessage.error('刪除失敗，請確認測試案例是否仍有關聯資料')
  }
}

onMounted(() => {
  fetchTestCase()
  fetchBugs()
})
</script>

<style scoped>
.testcase-detail-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 24px;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 20px;
  margin-bottom: 24px;
}

.header-left {
  display: flex;
  align-items: center;
  gap: 16px;
}

.header-actions {
  display: flex;
  align-items: center;
  gap: 8px;
}

.title {
  margin: 0;
  font-size: 24px;
  line-height: 1.3;
}

.subtitle {
  margin-top: 4px;
  font-size: 13px;
  color: #909399;
}

.detail-card {
  margin-bottom: 28px;
}

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 12px;
}

.section-title {
  display: flex;
  align-items: center;
  gap: 12px;
}

.section-title h3 {
  margin: 0;
}

.bug-count {
  font-size: 14px;
  color: #909399;
}

@media (max-width: 768px) {
  .header {
    align-items: flex-start;
    flex-direction: column;
  }

  .header-actions {
    width: 100%;
  }

  .section-header {
    align-items: flex-start;
    flex-direction: column;
    gap: 12px;
  }
}
</style>
